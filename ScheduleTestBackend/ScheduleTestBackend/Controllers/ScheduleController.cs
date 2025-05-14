﻿using BusinessAccesLayer.InterfaceServices;
using DataAccesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ScheduleTestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ISubjectService _subjectService;

        public ScheduleController(IScheduleService scheduleService, ISubjectService subjectService)
        {
            _scheduleService = scheduleService;
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedules>>> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedules();
            return Ok(schedules);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedules>> GetScheduleById(int id)
        {
            var userId = GetUserIdFromToken();

            var schedule = await _scheduleService.GetScheduleById(id);

            if (schedule.UserId != userId.Value)
            {
                return Unauthorized(new { message = "Không xác thực được người dùng" });
            }

            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertSchedule([FromBody] Schedules newSchedule)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Không xác thực được người dùng" });
                }

                var subjects = await _subjectService.GetAllSubjects(userId.Value);

                var subject = subjects.FirstOrDefault(s => s.Id == newSchedule.SubjectId);
                if (subject == null)
                {
                    return NotFound("Người dùng này không có môn học này, vui lòng thêm mới");
                }

                newSchedule.UserId = userId.Value;
                var result = await _scheduleService.InsertSchedule(newSchedule);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(410, new { mesage = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Schedules>> UpdateSchedule(Schedules newSchedule)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Không xác thực được người dùng" });
            }

            var schedules = await _scheduleService.GetSchedulesByUser(userId.Value);

            var schedule = schedules.FirstOrDefault(sc => sc.Id == newSchedule.Id);

            if (schedule == null)
            {
                return NotFound();
            }
            newSchedule.UserId = userId.Value;
            schedule = newSchedule;
            await _scheduleService.UpdateSchedule(schedule);

            return Ok(schedule);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchedule(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Không xác thực được người dùng" });
            }
            var schedules = await _scheduleService.GetSchedulesByUser(userId.Value);
            var schedule = schedules.FirstOrDefault(sc => sc.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            await _scheduleService.DeleteSchedule(id);
            return Ok(schedule);
        }

        [Authorize]
        [HttpGet("byUser")]
        public async Task<ActionResult> GetSchedulesByUser()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Không xác thực được người dùng" });
            }

            var schedules = await _scheduleService.GetSchedulesByUser(userId.Value);
            if (schedules == null)
            {
                return NotFound();
            }
            return Ok(schedules);
        }

        private int? GetUserIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var userIdClaim = userClaims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            return null;
        }

        [Authorize]
        [HttpGet("byDay/{dayOfWeek}")]
        public async Task<ActionResult<IEnumerable<Schedules>>> GetSchedulesByDay(int dayOfWeek)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Không xác thực được người dùng" });
                }

                var schedules = await _scheduleService.GetSchedulesByDayOfWeek(dayOfWeek);
                schedules = schedules.Where(s => s.UserId == userId.Value);
                return Ok(schedules);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("subjectName")]
        public async Task<ActionResult<IEnumerable<Schedules>>> SearchSchedulesBySubjectName([FromQuery] string keyword)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { message = "Không xác thực được người dùng" });

            var schedules = await _scheduleService.SearchSchedulesBySubjectName(userId.Value, keyword);
            return Ok(schedules);
        }

        [Authorize]
        [HttpGet("sessions-by-subject/{subjectId}")]
        public async Task<IActionResult> GetSessionsBySubject(int subjectId)
        {
            var sessions = (await _scheduleService.GetSessionsBySubjectAsync(subjectId)).ToList();

            var sessionList = sessions
                .Select((s, index) => new
                {
                    sessionNumber = index + 1,
                    startTime = s.StartTime.ToString(@"hh\:mm"),
                    endTime = s.EndTime.ToString(@"hh\:mm")
                })
                .ToList();

            var result = new
            {
                subjectId = subjectId,
                totalSessions = sessionList.Count,
                sessions = sessionList
            };

            return Ok(result);
        }

    }


}

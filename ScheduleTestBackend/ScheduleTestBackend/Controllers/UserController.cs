﻿using BusinessAccesLayer.InterfaceServices;
using DataAccesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ScheduleTestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IScheduleService _scheduleService;
        private readonly ISubjectService _subjectService;

        public UserController(IUserService userService,
            IScheduleService scheduleService,
            ISubjectService subjectService)
        {
            _userService = userService;
            _scheduleService = scheduleService;
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUser()
        {
            var users = await _userService.GetAllUser();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<Users>> GetUserById()
        {
            var userId = GetUserIdFromToken();
            var user = await _userService.GetUserById(userId.Value);
            if (user == null)
            {
                return Unauthorized("Không thể xác thực người dùng");
            }
            return Ok(user);
        }

        [HttpGet("EmailPassword")]
        public async Task<ActionResult<Users>> GetUserByEmailPassword(string email, string password)
        {
            var user = await _userService.GetUserByEmailPassword(email, password);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> InsertUser(Users newUser)
        {
            var userExist = await _userService.GetUserByEmail(newUser.Email);
            if (userExist != null)
            {
                return BadRequest("Email này đã tồn tại, vui lòng nhập một email khác.");
            }
            var user = await _userService.InsertUser(newUser);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Users>> UpdateUser([FromBody] Users updatedUser)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Không xác thực được người dùng!" });
            }

            var user = await _userService.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại!" });
            }

            user.FullName = updatedUser.FullName ?? user.FullName;
            user.Email = updatedUser.Email ?? user.Email;
            user.Password = updatedUser.Password ?? user.Password;

            await _userService.UpdateUser(user);

            return Ok(user);
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
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Không xác thực được người dùng!" });
            }

            var user = await _userService.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại!" });
            }

            await _userService.DeleteUser(user.Id);
            await _scheduleService.DeleteSchedulesInUser(user.Id);
            await _subjectService.DeleteSubjectsInUser(user.Id);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _userService.Login(model.Email, model.Password);
            if (token == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng!" });

            return Ok(new { Token = token });
        }


    }

}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ScheduleTestFrontend.Models;
using ScheduleTestFrontend.Services.IServices;

namespace ScheduleTestFrontend.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ISubjectService _subjectService;
        private string _token;
        //private string _token;

        public ScheduleController(IScheduleService scheduleService, ISubjectService subjectService)
        {
            _scheduleService = scheduleService;
            _subjectService = subjectService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _token = HttpContext.Session.GetString("AuthToken") ?? string.Empty;
        }

        ////Ban đầu
        //public async Task<IActionResult> Index()
        //{
        //    if (_token == "")
        //    {
        //        return RedirectToAction("Login", "User");
        //    }
        //    var schedules = await _scheduleService.GetScheduleByUserId(_token);
        //    if (schedules != null)
        //    {
        //        var subjects = await _subjectService.GetAllSubject(_token);
        //        var subjectDict = subjects.ToDictionary(s => s.Id, s => s.Name);

        //        foreach (var schedule in schedules)
        //        {
        //            if (subjectDict.TryGetValue(schedule.SubjectId, out var subjectName))
        //            {
        //                schedule.SubjectName = subjectName;
        //            }
        //        }

        //        schedules = schedules.OrderBy(s => s.DayOfWeek)
        //                             .ThenBy(s => s.StartTime)
        //                             .ToList();
        //    }

        //    return View(schedules);
        //}

        ////Tìm kiếm lịch học theo môn học
        //public async Task<IActionResult> Index(string searchKeyword)
        //{
        //    if (string.IsNullOrEmpty(_token))
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    var schedules = await _scheduleService.GetScheduleByUserId(_token);
        //    if (schedules != null)
        //    {
        //        var subjects = await _subjectService.GetAllSubject(_token);
        //        var subjectDict = subjects.ToDictionary(s => s.Id, s => s.Name);

        //        foreach (var schedule in schedules)
        //        {
        //            if (subjectDict.TryGetValue(schedule.SubjectId, out var subjectName))
        //            {
        //                schedule.SubjectName = subjectName;
        //            }
        //        }

        //        // ➤ Lọc theo từ khóa nếu có
        //        if (!string.IsNullOrWhiteSpace(searchKeyword))
        //        {
        //            schedules = schedules
        //                        .Where(s => s.SubjectName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
        //                        .ToList();
        //        }

        //        schedules = schedules
        //                    .OrderBy(s => s.DayOfWeek)
        //                    .ThenBy(s => s.StartTime)
        //                    .ToList();
        //    }

        //    ViewBag.SearchKeyword = searchKeyword;
        //    return View(schedules);
        //}


        ////Lấy danh sách lịch học theo thứ trong tuần
        //public async Task<IActionResult> Index(string searchKeyword, string dayOfWeek)
        //{
        //    if (string.IsNullOrEmpty(_token))
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    var schedules = await _scheduleService.GetScheduleByUserId(_token);
        //    if (schedules != null)
        //    {
        //        var subjects = await _subjectService.GetAllSubject(_token);
        //        var subjectDict = subjects.ToDictionary(s => s.Id, s => s.Name);

        //        foreach (var schedule in schedules)
        //        {
        //            if (subjectDict.TryGetValue(schedule.SubjectId, out var subjectName))
        //            {
        //                schedule.SubjectName = subjectName;
        //            }
        //        }

        //        // Lọc theo môn học
        //        if (!string.IsNullOrWhiteSpace(searchKeyword))
        //        {
        //            schedules = schedules
        //                .Where(s => s.SubjectName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
        //                .ToList();
        //        }

        //        // Lọc theo thứ trong tuần
        //        if (!string.IsNullOrWhiteSpace(dayOfWeek))
        //        {
        //            schedules = schedules
        //                .Where(s => s.DayOfWeek.ToString().Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase))
        //                .ToList();
        //        }

        //        schedules = schedules.OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).ToList();
        //    }

        //    ViewBag.SearchKeyword = searchKeyword;
        //    ViewBag.DayOfWeek = dayOfWeek;
        //    return View(schedules);
        //}

        //Đếm số lượng môn học 
        public async Task<IActionResult> Index(string searchKeyword, string dayOfWeek)
        {
            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Login", "User");
            }

            var schedules = await _scheduleService.GetScheduleByUserId(_token);
            if (schedules != null && schedules.Any())
            {
                var subjects = await _subjectService.GetAllSubject(_token);
                var subjectDict = subjects.ToDictionary(s => s.Id, s => s.Name);

                foreach (var schedule in schedules)
                {
                    if (subjectDict.TryGetValue(schedule.SubjectId, out var subjectName))
                    {
                        schedule.SubjectName = subjectName;
                    }
                }

                // Lọc theo môn học
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    schedules = schedules
                        .Where(s => s.SubjectName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Lọc theo thứ trong tuần
                if (!string.IsNullOrWhiteSpace(dayOfWeek))
                {
                    schedules = schedules
                        .Where(s => s.DayOfWeek.ToString().Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Sắp xếp theo thứ trong tuần và thời gian bắt đầu
                schedules = schedules.OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).ToList();

                // Đếm số lượng môn học
                ViewBag.TotalSubjects = schedules.Count();
            }
            else
            {
                ViewBag.TotalSubjects = 0;
            }

            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.DayOfWeek = dayOfWeek;
            return View(schedules);
        }


        public async Task<IActionResult> Insert()
        {
            await GetSubjectData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Schedule schedule)
        {
            await GetSubjectData();
            if (schedule.EndTime <= schedule.StartTime)
            {
                ModelState.AddModelError("EndTime", "Thời gian kết thúc phải sau thời gian bắt đầu.");
            }
            if (ModelState.IsValid)
            {
                var newSchedule = await _scheduleService.InsertSchedule(schedule, _token);
                if (newSchedule == null)
                {
                    TempData["ErrorMessage"] = "Thêm lịch học thất bại, có thể đã trùng lịch học.";
                    return View(schedule);
                }
                return RedirectToAction("Index", "Schedule");
            }
            return View(schedule);
        }

        public async Task<IActionResult> Update(int id)
        {
            var schedule = await _scheduleService.GetScheduleById(id, _token);
            await GetSubjectData();
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                await GetSubjectData();
                return View(schedule);
            }


            var newSchedule = await _scheduleService.UpdateSchedule(schedule, _token);
            if (newSchedule == null)
            {
                TempData["ErrorMessage"] = "Sửa lịch học thất bại";
                await GetSubjectData();
                return View(newSchedule);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _scheduleService.DeleteSchedule(id, _token);
            if (subject == null)
            {
                TempData["ErrorMessage"] = "Xóa lịch học thất bại";
            }

            return RedirectToAction("Index");
        }

        public async Task GetSubjectData()
        {
            var subjects = await _subjectService.GetAllSubject(_token);
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
        }
    }

}

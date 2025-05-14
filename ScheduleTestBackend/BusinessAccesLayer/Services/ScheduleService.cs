using BusinessAccesLayer.InterfaceServices;
using DataAccesLayer.Data;
using DataAccesLayer.Enum;
using DataAccesLayer.Models;
using DataAccesLayer.Repositories;
using DataAccesLayer.RepositoriesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccesLayer.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISubjectRepository _subjectRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<Schedules>> GetAllSchedules()
        {
            var schedules = await _scheduleRepository.GetAll();
            return schedules;
        }


        public async Task<IEnumerable<Schedules>> GetSchedulesByUser(int userId)
        {
            var schedules = await _scheduleRepository.GetSchedulesByUser(userId);
            return schedules;
        }

        public async Task<Schedules> GetScheduleById(int id)
        {
            var user = await _scheduleRepository.GetById(id);
            return user;
        }

        public async Task<Schedules> InsertSchedule(Schedules schedule)
        {
            var result = await _scheduleRepository.Insert(schedule);
            return result;
        }

        public async Task<Schedules> UpdateSchedule(Schedules schedule)
        {
            var result = await _scheduleRepository.UpdateSchedule(schedule);
            return result;
        }

        public async Task<Schedules> DeleteSchedule(int id)
        {
            var schedule = await _scheduleRepository.GetById(id);
            await _scheduleRepository.Delete(schedule);

            return schedule;
        }

        public async Task<IEnumerable<Schedules>> DeleteSchedulesInUser(int userId)
        {
            var schedules = await _scheduleRepository.DeleteSchedulesInUser(userId);
            return schedules;
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByDayOfWeek(int dayOfWeek)
        {
            if (!Enum.IsDefined(typeof(WeekDay), dayOfWeek))
            {
                throw new ArgumentException("Giá trị DayOfWeek không hợp lệ.");
            }

            var schedules = await _scheduleRepository.GetSchedulesByDayOfWeek((WeekDay)dayOfWeek);
            return schedules;
        }

        public async Task<IEnumerable<Schedules>> SearchSchedulesBySubjectName(int userId, string keyword)
        {
            return await _scheduleRepository.SearchSchedulesBySubjectName(userId, keyword);
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesBySubjectName(int userId, string subjectName)
        {
            return await _scheduleRepository.GetSchedulesBySubjectName(userId, subjectName);
        }

        public async Task<IEnumerable<Schedules>> GetSessionsBySubjectAsync(int subjectId)
        {
            return await _scheduleRepository.GetSchedulesBySubjectIdAsync(subjectId);
        }
    }

}



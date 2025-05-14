using DataAccesLayer.Models;
using DataAccesLayer.RepositoriesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccesLayer.InterfaceServices
{
    public interface IScheduleService
    {
        public Task<IEnumerable<Schedules>> GetAllSchedules();
        public Task<IEnumerable<Schedules>> GetSchedulesByUser(int userId);
        public Task<Schedules> GetScheduleById(int id);
        public Task<Schedules> InsertSchedule(Schedules schedule);
        public Task<Schedules> UpdateSchedule(Schedules schedule);
        public Task<Schedules> DeleteSchedule(int id);
        public Task<IEnumerable<Schedules>> DeleteSchedulesInUser(int userId);

        public Task<IEnumerable<Schedules>> GetSchedulesByDayOfWeek(int dayOfWeek);

        public Task<IEnumerable<Schedules>> SearchSchedulesBySubjectName(int userId, string keyword);

        public Task<IEnumerable<Schedules>> GetSchedulesBySubjectName(int userId, string subjectName);
        public Task<IEnumerable<Schedules>> GetSessionsBySubjectAsync(int subjectId);
    }


}

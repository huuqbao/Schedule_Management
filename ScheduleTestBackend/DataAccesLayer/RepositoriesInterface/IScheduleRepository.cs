using DataAccesLayer.Enum;
using DataAccesLayer.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.RepositoriesInterface
{
    public interface IScheduleRepository : IGenericRepository<Schedules>
    {
        public Task<IEnumerable<Schedules>> GetSchedulesByUser(int userId);
        public Task<Schedules> UpdateSchedule(Schedules schedule);
        public Task<IEnumerable<Schedules>> DeleteSchedulesInUser(int userId);

        public Task<IEnumerable<Schedules>> GetSchedulesByDayOfWeek(WeekDay dayOfWeek);

        public Task<IEnumerable<Schedules>> SearchSchedulesBySubjectName(int userId, string keyword);
        public Task<IEnumerable<Schedules>> GetSchedulesBySubjectName(int userId, string subjectName);
        public Task<IEnumerable<Schedules>> GetSchedulesBySubjectIdAsync(int subjectId);

    }


}

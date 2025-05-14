using DataAccesLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.RepositoriesInterface
{
    public interface ISubjectRepository : IGenericRepository<Subjects>
    {
        public Task<IEnumerable<Subjects>> GetAllSubjectsFollowUser(int userId);
        public Task<bool> CheckSubjectExistInSchedule(int subjectId);
        public Task<IEnumerable<Subjects>> DeleteSubjectsInUser(int userId);
        public Task<IEnumerable<Subjects>> SearchSubjectsByName(int userId, string keyword);

        public Task<IEnumerable<Schedules>> SearchSchedulesBySubjectName(int userId, string keyword);
    }


}

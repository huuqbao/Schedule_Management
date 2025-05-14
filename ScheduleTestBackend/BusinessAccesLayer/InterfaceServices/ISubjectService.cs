using DataAccesLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccesLayer.InterfaceServices
{
    public interface ISubjectService
    {
        public Task<IEnumerable<Subjects>> GetAllSubjects(int userId);
        public Task<Subjects> GetSubjectById(int id);
        public Task<Subjects> InsertSubject(Subjects subject);
        public Task<Subjects> UpdateSubject(Subjects subject);
        public Task<Subjects> DeleteSubject(int id);
        public Task<bool> CheckSubjectExistInSchedule(int subjectId);
        public Task<IEnumerable<Subjects>> DeleteSubjectsInUser(int userId);
        public Task<IEnumerable<Subjects>> SearchSubjectsByName(int userId, string keyword);

        public Task<IEnumerable<Schedules>> SearchSchedulesBySubjectName(int userId, string keyword);
    }

}

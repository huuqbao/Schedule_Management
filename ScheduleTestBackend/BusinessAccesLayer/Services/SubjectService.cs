using BusinessAccesLayer.InterfaceServices;
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
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<Subjects>> GetAllSubjects(int userId)
        {
            var subjects = await _subjectRepository.GetAllSubjectsFollowUser(userId);
            return subjects;
        }

        public async Task<Subjects> GetSubjectById(int id)
        {
            var subject = await _subjectRepository.GetById(id);
            return subject;
        }

        public async Task<Subjects> InsertSubject(Subjects subject)
        {
            var result = await _subjectRepository.Insert(subject);
            return result;
        }

        public async Task<Subjects> UpdateSubject(Subjects subject)
        {
            var result = await _subjectRepository.Update(subject);
            return result;
        }

        public async Task<Subjects> DeleteSubject(int id)
        {
            var subject = await _subjectRepository.GetById(id);
            await _subjectRepository.Delete(subject);
            return subject;
        }

        public async Task<bool> CheckSubjectExistInSchedule(int subjectId)
        {
            return await _subjectRepository.CheckSubjectExistInSchedule(subjectId);
        }

        public async Task<IEnumerable<Subjects>> DeleteSubjectsInUser(int userId)
        {
            return await _subjectRepository.DeleteSubjectsInUser(userId);
        }

        public async Task<IEnumerable<Subjects>> SearchSubjectsByName(int userId, string keyword)
        {
            return await _subjectRepository.SearchSubjectsByName(userId, keyword);
        }

        public async Task<IEnumerable<Schedules>> SearchSchedulesBySubjectName(int userId, string keyword)
        {
            return await _subjectRepository.SearchSchedulesBySubjectName(userId, keyword);
        }


    }
}

using ScheduleTestFrontend.Models.Enum;
using System.Diagnostics.CodeAnalysis;

namespace ScheduleTestFrontend.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        [AllowNull]
        public string SubjectName { get; set; }
        public WeekDay DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        //public virtual Subject Subjects { get; set; }
    }

}

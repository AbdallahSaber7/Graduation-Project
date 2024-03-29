using System.ComponentModel.DataAnnotations;

namespace CHS.DAL.Entites
{
    public class Semester
    {

        [Key]
        [MaxLength(100, ErrorMessage = "Semester title too long")]
        public string SemesterTitle { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public virtual ICollection<CourseHasSemester> courseHasSemesters { get; set; } = new List<CourseHasSemester>();
        public virtual TimeTable? TimeTable{ get; set; }
        public ICollection<CourseGroup>? courseGroups{ get; set; }
        public virtual ICollection<StudentTracker> StudentTrackers { get; set; } = new List<StudentTracker>();
    }
}

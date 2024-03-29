using CHS.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHS.DAL.Entites
{
    public class CourseHasSemester
    {
        [ForeignKey("Semester")]
        public string SemsterTitle { get; set; }
        [ForeignKey("Course")]
        public string CourseId { get; set; }
        public int NumberOfStudentsEnrolled { get; set; }
        public Semester Semester { get; set; }
        public Course Course { get; set; }
    }
}

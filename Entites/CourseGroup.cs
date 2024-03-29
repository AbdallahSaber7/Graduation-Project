using CHS.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace CHS.DAL.Entites
{
    public class CourseGroup
    {
        public int Id { get; set; }
        [ForeignKey("Course")]
        public string CourseCode { get; set; }
        [ForeignKey("Semester")]
        public string SemesterTitle { get; set; }
        public string Group { get; set; }
        public int Capacity { get; set; }
        public int CurrentCapacity { get; set; }
        public virtual  Semester? Semester { get; set; }
        public virtual Course? Course  { get; set; }
        public virtual ICollection<Enroll>? enrolls{ get; set; } = new List<Enroll>();
        public virtual ICollection<Record> Records { get; set; } = new List<Record>();
        public virtual ICollection<Evaluate>? Evaluates { get; set; } = new List<Evaluate>();

        //public virtual Teach? Teach { get; set; }
    }
}

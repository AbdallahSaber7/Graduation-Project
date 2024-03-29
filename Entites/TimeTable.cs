using System.ComponentModel.DataAnnotations.Schema;

namespace CHS.DAL.Entites
{
    public class TimeTable
    {
        public int Id { get; set; }
        public int Year { get; set; }
        [ForeignKey("Semester")]
        public string SemesterTitle { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual ICollection<Lecture>? Lectures { get; set; } = new List<Lecture>();


    }
}

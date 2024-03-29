using System.ComponentModel.DataAnnotations.Schema;
namespace CHS.DAL.Entities
{
    public class StudentFinishedCourses
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public string FinishedCourses { get; set; }//where??
        public virtual Student Student { get; set; }
    }
}
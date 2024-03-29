using System.ComponentModel.DataAnnotations;

namespace CHS.DAL.Entites
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        public string InstructorName { get; set; }
        public string? InstructorPassword{ get; set; }
        [EmailAddress]
        public string? InstructorEmail { get; set; }
        
        public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();
        public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    }
}

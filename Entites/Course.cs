using CHS.DAL.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CHS.DAL.Entities
{
    public class Course
    {
        [Required]
        [MaxLength(5, ErrorMessage = "Course code is too long")]
        [Key]
        public string CourseCode { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Title is too long")]
        public string CourseTitle { get; set; }
        [Required]
        public int CreditHours { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description is too long")]
        public string Description { get; set; }
        [MaxLength(50, ErrorMessage = "Category is too long")]
        public string? Category { get; set; }
        [MaxLength(30, ErrorMessage = "Mandatorness is too long")]
        public string Mandatorness { get; set; }
        // i want to create a prequisite property that is foreign key to the same table a recursive relationship using data annotations
        [ForeignKey("PrerequisiteCourse")]
        public string? PrerequisiteCode { get; set; }
        public virtual Course? PrerequisiteCourse { get; set; }
       // public virtual ICollection<Evaluate>? Evaluates { get; set; } = new List<Evaluate>();
        public virtual ICollection<CourseHasSemester>? courseHasSemesters { get; set; } = new List<CourseHasSemester>();
        public virtual ICollection<CourseGroup>? CourseGroups { get; set; } = new List<CourseGroup>();

    }
}
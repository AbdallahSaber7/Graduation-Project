using CHS.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHS.DAL.Entites
{
    public class Evaluate
    {
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("CourseGroup")]
        public int CourseGroupId { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        [Required]
        public decimal PreFinalGrade { get; set; } = 0;
        [Column(TypeName = "decimal(4, 2)")]
        [Required]
        public decimal MidtermGrade { get; set; } = 0;
        [Column(TypeName = "decimal(4, 2)")]
        public decimal AbsencePercent { get; set; } = 0;
        [Column(TypeName = "decimal(4, 2)")]
        [Required]
        public decimal FinalGrade { get; set; } = 0;
        [Column(TypeName = "decimal(5, 2)")]
        [Required]
        public decimal TotalGrade { get; set; } = 0;
        [Column(TypeName = "decimal(5, 2)")]
        [Required]
        public decimal WeekTwelve { get; set; } = 0;
        public string LetterGrade  { get; set; } = "U";
        [Column(TypeName = "decimal(4, 2)")]
        public decimal PointsGrade { get; set; } = 0;
        public virtual CourseGroup CourseGroup { get; set; }
        public virtual Student Student { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}

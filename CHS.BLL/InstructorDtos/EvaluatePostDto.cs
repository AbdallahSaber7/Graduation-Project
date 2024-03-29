using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CHS.BLL.InstructorDtos
{
    public class EvaluatePostDto
    {
        public int StudentId { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        [Range(0, 10, ErrorMessage = "The value must be between 0 and 10.")]
        public decimal MidtermGrade { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        [Range(0, 10, ErrorMessage = "The value must be between 0 and 10.")]
        public decimal PreFinalGrade { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        [Range(0, 20, ErrorMessage = "The value must be between 0 and 20.")]
        public decimal WeekTwelve { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        [Range(0, 60, ErrorMessage = "The value must be between 0 and 60.")]
        public decimal FinalGrade { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        [Range(0, 10, ErrorMessage = "The value must be between 0 and 10.")]
        public decimal AbsencePercent { get; set; }
        //[Column(TypeName = "decimal(4, 2)")]
        //[Range(0, 100, ErrorMessage = "The value must be between 0 and 100.")]
        //public decimal TotalGrade{ get; set; }
        //public string? LetterGrade { get; set; }

    }
}

namespace CHS.BLL.InstructorDtos
{
    public class EvaluateDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public decimal MidtermGrade { get; set; }
        public decimal PreFinalGrade { get; set; } 
        public decimal WeekTwelve { get; set; }
        public decimal FinalGrade { get; set; }
        public decimal AbsencePercent { get; set; }
        public decimal TotalGrade { get; set; }
        public string? LetterGrade { get; set; }
        public decimal PointsGrade { get; set; }
    }
}

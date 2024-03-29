namespace CHS.BLL.StudentDtos
{
    public class FinishedCourseDto
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public decimal PointsGrade { get; set; }
        public string LetterGrade{ get; set; }
        public int CreditHours { get; set; }
    }
}

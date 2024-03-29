namespace CHS.BLL.StudentDtos
{
    public class StudentResultsDto
    {
        public decimal AccumulativeGPA { get; set; }
        public decimal SemesterGPA { get; set; }
        public List<StudentListResultsDto> StudentListResults { get; set; } = new List<StudentListResultsDto>();
    }
}

namespace CHS.BLL.InstructorDtos
{
    public class AbsencePostDto
    {
        public int StudentId { get; set; }
        public IEnumerable<int> Week { get; set; } = new List<int>(); 
    }
}

namespace CHS.BLL.StudentDtos
{
    public class StudentFinishedCoursesDto
    {
        public int FinishedCreditHours { get; set; }
        public int UnfinishedCreditHours
        {
            get 
            {
                return 135 - FinishedCreditHours;
            }
        } 
        public List<FinishedCourseDto> FinishedCourses { get; set; }
        public List<UnfinishedCourseDto> UnfinishedCourses { get; set; }
    }
}

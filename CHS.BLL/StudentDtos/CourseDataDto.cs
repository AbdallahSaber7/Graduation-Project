namespace CHS.BLL.StudentDtos
{
    public class CourseDataDto
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string CourseGroups { get; set; } 
        public int CreditHours { get; set; }
        public string? PrerequisiteCourseCode { get; set; }
        public string LectureDay { get; set; }
        public string LectureStartTime { get; set; }
        public string LectureEndTime { get; set; }
    }

}


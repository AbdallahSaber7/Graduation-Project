using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.BLL.StudentDtos
{
    public class TimeTableStudentDto
    {
        public string Type { get; set; }
        public string InsName { get; set; }
        public string CourseTitle { get; set; }
        public string Group { get; set; }
        public string RoomName { get; set; }
        public string Day { get; set; }
        public string StartInterval { get; set; }
        public string EndInterval { get; set; }
    }
}

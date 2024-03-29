using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.BLL.StudentDtos
{
    public class EnrollmentRequestDto
    {
        public int StudentId { get; set; }
        public string CourseCode { get; set; }
        public string SemesterTitle { get; set; }
        public string Group { get; set; }
    }
}

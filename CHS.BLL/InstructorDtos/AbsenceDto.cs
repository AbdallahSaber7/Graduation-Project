using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.BLL.InstructorDtos
{
    public class AbsenceDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public List <int> Weeks { get; set; } = new List<int>();
    }
}

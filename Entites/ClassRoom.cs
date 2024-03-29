using CHS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites
{
    public class ClassRoom
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RoomName { get; set; }
        [Required]
        [DefaultValue(30)]
        public int Capacity { get; set; } 
        public string classCategory{ get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
        public virtual ICollection<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

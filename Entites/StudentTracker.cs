using CHS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites
{
    public class StudentTracker
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Semester")]
        public string SemesterTitle { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal SemesterGPA { get; set; }
        public int MaximumHours { get; set; }
        public int RecordedHours { get; set; }
        public int Tracker { get; set; }
        public virtual Student Student { get; set; }

        public virtual Semester Semester { get; set; }
    }
}

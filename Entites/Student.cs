using CHS.DAL.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entities
{
    public class Student
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Name is too long")]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public int FinishedHours { get; set; }
        [Required]
        [Column(TypeName = "decimal(3, 2)")]
        public decimal AccumulativeGpa { get; set; }
        public virtual ICollection<StudentFinishedCourses> StudentFinishedCourses { get; set; } = new List<StudentFinishedCourses>(); 
        public virtual ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
        public virtual ICollection<Record> Records { get; set; } = new List<Record>();
        public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();
        public virtual ICollection<StudentTracker> StudentTrackers { get; set; } = new List<StudentTracker>();


    }
}
using CHS.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace CHS.DAL.Entites
{
    public class Enroll
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("CourseGroup")]
        public int CourseGroupId { get; set; }
        public virtual Student? Student { get; set; }
        public virtual CourseGroup? CourseGroup { get; set; }
    }
}

using CHS.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHS.DAL.Entites
{
    public class Record
    {
        [ForeignKey("CourseGroup")]
        public int CourseGroupId { get; set; }
        [ForeignKey("Student")]

        public int StudentId { get; set; }
        //[ForeignKey("Lecture")]

        //public int LectureId { get; set; }
        public int Week { get; set; }
        public virtual CourseGroup? CourseGroup{ get; set; }

        public virtual Student? Student { get; set; }

       // public virtual Lecture? Lecture { get; set; }
    }
}

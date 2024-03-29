using CHS.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHS.DAL.Entites
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }
        public string Day { get; set; }
        public string? Type { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; }

        [ForeignKey("ClassRoom")]
        public int ClassRoomId { get; set; }
        public virtual ClassRoom? ClassRoom { get; set; }
        [ForeignKey("TimeTable")]
        public int TimeTableId { get; set; }
        public virtual TimeTable? TimeTable { get; set; }
        [ForeignKey("CourseGroup")]
        public int CourseGroupId { get; set; }
        public virtual CourseGroup? CourseGroup { get; set; }
        //public ICollection<Record> Records { get; set; } = new List<Record>();
        [ForeignKey("Interval")]
        public int IntervalIdFk { get; set; }
        public virtual Interval? Interval{ get; set; }
    }
}

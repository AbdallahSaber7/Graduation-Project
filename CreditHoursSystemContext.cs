using CHS.DAL.Entites;
using CHS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CHS.DAL
{
    public class CreditHoursSystemContext : DbContext
    {
        public CreditHoursSystemContext(DbContextOptions<CreditHoursSystemContext> options) : base(options)
        {
        }
        // Add DbSet properties here
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentFinishedCourses> StudentFinishedCourses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Record> Records { get; set; }
        //public DbSet<Teach> Teaches { get; set; }
        public DbSet<Evaluate> Evaluates { get; set; }
        public DbSet<CourseHasSemester> CourseHasSemesters { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<Interval> Intervals{ get; set; }
        public DbSet<StudentTracker> studentTrackers{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

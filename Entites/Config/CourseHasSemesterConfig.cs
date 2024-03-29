using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CHS.DAL.Entites.Config
{
    public class CourseHasSemesterConfig : IEntityTypeConfiguration<CourseHasSemester>
    {
        public void Configure(EntityTypeBuilder<CourseHasSemester> builder)
        {
            //two primaryKey studentId and finishedCourses
            builder.HasKey(sc => new { sc.SemsterTitle, sc.CourseId });
        }
    }
}

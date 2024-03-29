using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHS.DAL.Entites.Config
{
    public class StudentTrackerConfig : IEntityTypeConfiguration<StudentTracker>
    {
        public void Configure(EntityTypeBuilder<StudentTracker> builder)
        {
            builder.HasKey(st => new { st.StudentId, st.SemesterTitle });
        }

    }
}

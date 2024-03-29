using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites.Config
{
    public class EvaluateConfig : IEntityTypeConfiguration<Evaluate>
    {
        public void Configure(EntityTypeBuilder<Evaluate> builder)
        {
            //three primaryKey studentId and finishedCourses
            builder.HasKey(sc => new { sc.StudentId, sc.CourseGroupId, sc.InstructorId });
        }
    }
}

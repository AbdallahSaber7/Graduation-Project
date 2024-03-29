using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites.Config
{
    public class RecordConfig : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            //three primaryKey studentId and finishedCourses
            builder.HasKey(sc => new { sc.CourseGroupId, sc.StudentId, sc.Week });
        }
    }
}

using CHS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites.Config
{
    public class EnrollConfig : IEntityTypeConfiguration<Enroll>
    {
        public void Configure(EntityTypeBuilder<Enroll> builder)
        {
            //two primaryKey studentId and finishedCourses
            builder.HasKey(sc => new { sc.StudentId, sc.CourseGroupId });
        }


    }
}

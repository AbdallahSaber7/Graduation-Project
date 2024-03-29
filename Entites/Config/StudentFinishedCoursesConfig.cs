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
    public class StudentFinishedCoursesConfig : IEntityTypeConfiguration<StudentFinishedCourses>
    {
        public void Configure(EntityTypeBuilder<StudentFinishedCourses> builder)
        {
            //two primaryKey studentId and finishedCourses
            builder.HasKey(sc => new { sc.StudentId, sc.FinishedCourses });
        }
    }
}

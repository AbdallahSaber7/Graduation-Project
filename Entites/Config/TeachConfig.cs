//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CHS.DAL.Entites.Config
//{
//    public class TeachConfig : IEntityTypeConfiguration<Teach>
//    {
//        public void Configure(EntityTypeBuilder<Teach> builder)
//        {
//            //two primaryKey studentId and finishedCourses
//            // builder.HasKey(sc => new { sc.InstructorId, sc.CourseGroupId });
//            builder.Property(e => e.Id)
//            .ValueGeneratedOnAdd()
//            .UseIdentityColumn(1, 1);
//        }
//    }
//}

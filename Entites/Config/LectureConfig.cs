using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites.Config
{
    public class LectureConfig : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(1, 1);
        }
    }

}


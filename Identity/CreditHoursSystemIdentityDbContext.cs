using CHS.DAL.Entites.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Identity
{
    public class CreditHoursSystemIdentityDbContext : IdentityDbContext<AppUser>
    {
        public CreditHoursSystemIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHS.DAL.Entites.Identity
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? InstructorType { get; set; }
    }
}

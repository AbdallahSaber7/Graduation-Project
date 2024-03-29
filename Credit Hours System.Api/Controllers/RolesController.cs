using CHS.DAL.Entites.Identity;
using Credit_Hours_System.Api.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Hours_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        #region AddRoles and getRoles
        [HttpGet("GetRules")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = roleManager.Roles;
            return Ok(roles);
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(RolesDto role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = role.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return Ok(identityRole);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return BadRequest(role);
        }
        #endregion
        #region UpdateRole
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(string id , IdentityRole role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var AppRole = await roleManager.FindByIdAsync(id);
                    AppRole.Name = role.Name;
                    AppRole.NormalizedName = role.Name.ToUpper();
                    var result = await roleManager.UpdateAsync(AppRole);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return Ok(role);

        }
        #endregion
        #region DeleteRole
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id , IdentityRole role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }          
            try
            {
                var AppRole = await roleManager.FindByIdAsync(id);
                var result = await roleManager.DeleteAsync(AppRole);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(role);
        }
        #endregion

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Const;

namespace SchoolAPI.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =Roles.Admin)]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> CreateRoleAsync(string role)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
            return Ok();
        }
    }

}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPI.Const;
using SchoolAPI.DTO;
using SchoolAPI.Model;
using SchoolAPI.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IStudentRepository student;
        private readonly ITeacherRepository teacher;

        public AccountController(UserManager<ApplicationUser> userManager , IConfiguration config , 
            IStudentRepository student , ITeacherRepository teacher)
        {
            this.userManager = userManager;
            this.config = config;
            this.student = student;
            this.teacher = teacher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync( RegisterUserDTO newUser )
        {
            ApplicationUser userModel = new ApplicationUser();
            userModel.Email = newUser.Email;
            userModel.UserName = newUser.UserName;
            userModel.PhoneNumber = newUser.Phone;
            IdentityResult res = await userManager.CreateAsync(userModel , newUser.Password);
            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(userModel, "Student");
                ApplicationUser user = await userManager.FindByEmailAsync(userModel.Email);
                string id = user.Id;
                await student.CreateStudent(newUser , id);
                return Ok("Done");
            }
            return BadRequest(res.Errors.First());
        }

        [Authorize(Roles =Roles.Admin)]
        [HttpPost("RegisterTeacher")]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherDTO newUser)
        {
            ApplicationUser userModel = new ApplicationUser();
            userModel.Email = newUser.Email;
            userModel.UserName = newUser.UserName;
            userModel.PhoneNumber = newUser.Phone;
            IdentityResult res = await userManager.CreateAsync(userModel, newUser.Password);
            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(userModel, "Teacher");
                ApplicationUser user = await userManager.FindByEmailAsync(userModel.Email);
                string id = user.Id;
                await teacher.CreateTeacher(newUser , id);
                return Ok("Done");
            }
            return BadRequest(res.Errors.First());
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin(AdminDTO newAdmin)  
        {
            ApplicationUser userModel = new ApplicationUser();
            userModel.Email = newAdmin.Email;
            userModel.UserName = newAdmin.UserName;
            userModel.PhoneNumber = newAdmin.Phone;
            IdentityResult res = await userManager.CreateAsync(userModel, newAdmin.Password);
            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(userModel, "Admin");
                return Ok("Done");
            }
            return BadRequest(res.Errors.First());
        }

        [HttpPost("Login")]
//      [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync( LoginDTO userDTO)
        {
            ApplicationUser userModel = await userManager.FindByNameAsync(userDTO.UserName);
            if(userModel != null && await userManager.CheckPasswordAsync(userModel, userDTO.Password) )
            {
                List<Claim> MyClaims = new List<Claim>();
                MyClaims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
                MyClaims.Add( new Claim( ClaimTypes.NameIdentifier, userModel.Id) );
                MyClaims.Add( new Claim( JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString() ));

                List<string> Roles = ( List<string> )await userManager.GetRolesAsync(userModel);

                if(Roles != null)
                {
                    foreach(var role in Roles)
                    {
                        MyClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecrytKey"]));
                SigningCredentials credintional = new SigningCredentials( authKey , SecurityAlgorithms.HmacSha256 );

                JwtSecurityToken myToken = new JwtSecurityToken(
                    issuer: config["JWT:ValidIss"],
                    audience: config["JWT:ValidAud"],
                    expires: DateTime.Now.AddHours(2),
                    claims: MyClaims,
                    signingCredentials: credintional
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(myToken),
                    expire = myToken.ValidTo,
                }) ;

            }
            return BadRequest("Invalid Data");

        }

       

    }
}


/*
{
  "userName": "khaled",
  "password": "mmMM123@!",
  "email": "khaled25@gmail.com",
  "phone": "01133458967"
}
*/
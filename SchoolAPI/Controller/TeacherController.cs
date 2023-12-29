using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Const;
using SchoolAPI.DTO;
using SchoolAPI.Model;
using SchoolAPI.Repository;
using System.Security.Claims;

namespace SchoolAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
       
        private readonly ITeacherRepository teacher;

        public TeacherController(ITeacherRepository teacher)
        {
            this.teacher = teacher;
        }

        [Authorize(Roles =Roles.Admin)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await teacher.GetAllTeachers());
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetTeacherByIdAsync(int id)
        {
            return Ok(await teacher.GetTeacherById(id));
        }

        [Authorize(Roles = Roles.Teacher)]
        [HttpGet("GetInfo")]
        public async Task<IActionResult> GetTeacherInfoAsync()
        {
            string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok( await teacher.GetTeacherInfo(Id) );
        }

        /*
        [HttpPost("Create")]
        public async Task<IActionResult> AddTeacherAsync(RegisterUserDTO newTeacher)
        {
            return Ok(await teacher.CreateTeacher(newTeacher));
        }
        */

        [Authorize(Roles = Roles.Teacher)]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateTeacherAsync(int id, RegisterTeacherDTO newTeacher)
        {
            return Ok(await teacher.UpdateTeacher(id, newTeacher));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteStudentAsync(int id)
        {
            return Ok(await teacher.DeleteTeacher(id));
        }


    }

}

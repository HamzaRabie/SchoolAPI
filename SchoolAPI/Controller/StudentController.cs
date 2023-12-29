using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class StudentController : ControllerBase
    {
      
        private readonly IStudentRepository student;

        public StudentController( IStudentRepository student )
        {
            this.student = student;
        }

        [Authorize(Roles =Roles.Admin)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok (await student.GetAllStudents());
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetStudentByIdAsync(int id)
        {
            return Ok(await student.GetStudentById(id));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("GetInfo")]
        public async Task<IActionResult> GetStudentInfoAsync()
        {
            string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok( await student.GetStudentInfo(Id));
        }
        /*
        [HttpPost("Create")]
        public async Task<IActionResult> AddStudentAsync(RegisterUserDTO newStudent)
        {
            return Ok(await student.CreateStudent(newStudent));
        }
        */

        [Authorize(Roles=Roles.Student)]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateStudentAsync(int id , RegisterUserDTO newStudent)
        {
            return Ok(await student.UpdateStudent(id , newStudent));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteStudentAsync(int id)
        {
            return Ok(await student.DeleteStudent(id));
        }

        [Authorize(Roles =Roles.Student)]
        [HttpPost("Enroll")]
        public async Task<IActionResult> EnrollAsync( [FromBody] string courseName)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok( await student.Enroll( studentId , courseName) );
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPost("Disenroll")]
        public async Task<IActionResult> DisenrollAsync([FromBody] string courseName)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await student.Disenroll(studentId, courseName));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("GetCourse")]
        public async Task<IActionResult> GetStudentCoursesAsync()
        {
            string studentId = User.FindFirstValue(ClaimTypes.NameIdentifier) ;
            return Ok( await student.GetCourseList(studentId) );
        }

    }
}

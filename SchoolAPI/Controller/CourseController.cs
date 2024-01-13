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
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository course;

        public CourseController(ICourseRepository course)
        {
            this.course = course;
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCoursesAsync()
        {
            return Ok(await course.GetAllCourses());
        }

        [Authorize]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetCourseByIdAsync(int id)
        {
            return Ok(await course.GetCourseById(id));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourseAsync(CourseDTO newCourse )
        {
            if(ModelState.IsValid)
            {
                return Ok( await course.CreateCourse(newCourse));
            }
            else 
                return BadRequest();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourseAsync (int id , CourseDTO newCourse )
        {
            if (ModelState.IsValid)
            {
                return Ok( await course.UpdateCourse(id ,newCourse) );

            }
            else
                return BadRequest();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCourseAsync (int id)
        {
            return Ok( await course.DeleteCourse(id) );
        }

        [Authorize(Roles = Roles.Teacher)]
        [HttpPost("GetCourseStudents")]
        public async Task<IActionResult> GetCourseStudents( [FromBody] string courseName )
        {
            string currTeacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok( await course.GetCourseStudents(courseName , currTeacherId) );
        }

        
    }
}

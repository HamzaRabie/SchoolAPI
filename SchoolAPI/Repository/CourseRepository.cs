using Microsoft.EntityFrameworkCore;
using SchoolAPI.DB;
using SchoolAPI.DTO;
using SchoolAPI.Model;

namespace SchoolAPI.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext context;
        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Course> CreateCourse(CourseDTO newCourse)
        {
           Course courseModel = new Course();
           courseModel.Description = newCourse.Description;
           courseModel.Name = newCourse.Name;
           courseModel.Year = newCourse.Year;
           
           await context.Courses.AddAsync(courseModel);
           context.SaveChanges();
           return courseModel;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            Course oldCourse = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if( oldCourse != null)
            {
                context.Courses.Remove(oldCourse);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
            
        }

        public async Task<List<CourseListDTO>> GetAllCourses()
        {
            List<Course> courses = await context.Courses.OrderBy(c => c.Year).ToListAsync();
            List<CourseListDTO> courseList = new List<CourseListDTO>();
            foreach (var course in courses)
            {
                courseList.Add(new CourseListDTO()
                {
                    Name = course.Name,
                    Year = course.Year,
                    Description = course.Description,
                });
            }
            return courseList;
        }

        public async Task<CourseListDTO> GetCourseById(int id)
        {
            Course course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course != null)
            {
                CourseListDTO courseDTO = new CourseListDTO();
                courseDTO.Year = course.Year;
                courseDTO.Description = course.Description;
                courseDTO.Name = course.Name;
                return courseDTO;
            }
            else
                return null;

        }

        public async Task<Course> UpdateCourse(int id, CourseDTO newCourse)
        {
            Course oldCourse = await context.Courses.FirstOrDefaultAsync(c=> c.Id == id);
            if (oldCourse != null)
            {
                oldCourse.Name = newCourse.Name;
                oldCourse.Description = newCourse.Description;
                oldCourse.Year = newCourse.Year;
                await context.SaveChangesAsync();
                return oldCourse;
            }
            return null;

        }

        public async Task<List<StudentCourseDTO>> GetCourseStudents(string courseName , string teacherId )
        {
            Teacher currTeacher = context.Teachers.FirstOrDefault( e => e.AppUserId == teacherId );
            Course course = await context.Courses.Include(c=>c.Students).FirstOrDefaultAsync(c => c.Name == courseName);
            if (currTeacher.CourseID != course.Id) return null;
            List<StudentCourseDTO> students = new List<StudentCourseDTO>();
            if(course != null)
            {
                foreach (var item in course.Students)
                {
                    students.Add(new StudentCourseDTO()
                    {
                        UserName = item.Name,
                        Email = item.Email,
                    });
                }
                return students;
            }
            return null;
           

            
        }
    }
}

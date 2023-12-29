using SchoolAPI.DTO;
using SchoolAPI.Model;

namespace SchoolAPI.Repository
{
    public interface ICourseRepository
    {
        Task<List<CourseListDTO>> GetAllCourses();

        Task<CourseListDTO> GetCourseById(int id);

        Task<Course> CreateCourse(CourseDTO newCourse);

        Task<Course> UpdateCourse(int id, CourseDTO newCourse);
        Task<bool> DeleteCourse(int id);
        Task<List<StudentCourseDTO>> GetCourseStudents(string courseName, string teacherId);
    }
}

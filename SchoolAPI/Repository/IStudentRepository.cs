using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTO;
using SchoolAPI.Model;

namespace SchoolAPI.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudents();

        Task<Student> GetStudentById(int id);

        Task<RegisterUserDTO> GetStudentInfo(string userId);

        Task<Student> CreateStudent(RegisterUserDTO newstudent , string id);

        Task<Student>  UpdateStudent(int id , RegisterUserDTO newStudent);

        Task<bool>  DeleteStudent(int id);

        Task<string> Enroll(string Userid, string courseName);

        Task<List<CourseListDTO>> GetCourseList(string userId);

        Task<string> Disenroll(string Userid, string courseName);
    }
}

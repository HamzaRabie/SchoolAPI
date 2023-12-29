using SchoolAPI.DTO;
using SchoolAPI.Model;

namespace SchoolAPI.Repository
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetAllTeachers();

        Task<Teacher> GetTeacherById(int id);

        Task<RegisterTeacherDTO> GetTeacherInfo(string userId);

        Task<Teacher> CreateTeacher(RegisterTeacherDTO newTeacher, string id);

        Task<Teacher> UpdateTeacher(int id, RegisterTeacherDTO newTeacher);

        Task<bool> DeleteTeacher(int id);
    }
}

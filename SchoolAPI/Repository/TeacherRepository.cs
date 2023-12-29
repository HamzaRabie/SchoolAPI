using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.DB;
using SchoolAPI.DTO;
using SchoolAPI.Model;

namespace SchoolAPI.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public TeacherRepository(AppDbContext context , UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Teacher> CreateTeacher(RegisterTeacherDTO newTeacher , string id)
        {
            Teacher teacher = new Teacher();
            teacher.Name = newTeacher.UserName;
            teacher.Phone = newTeacher.Phone;
            teacher.Email = newTeacher.Email;
            teacher.CourseID = newTeacher.CourseID;
            teacher.Password = newTeacher.Password;
            teacher.AppUserId = id;
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();


            return teacher;
        }

        public async Task<bool> DeleteTeacher(int id)
        {
            Teacher existTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (existTeacher != null)
            {
                ApplicationUser teacherASP = await userManager.FindByEmailAsync(existTeacher.Email);
                context.Teachers.Remove(existTeacher);
                await userManager.DeleteAsync(teacherASP);
                await context.SaveChangesAsync();
                return true;

            }
            return false;

        }

        public async Task<List<Teacher>> GetAllTeachers()
        {
            return (await context.Teachers.ToListAsync());
        }

        public async Task<Teacher> GetTeacherById(int id)
        {
            return (await context.Teachers.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<RegisterTeacherDTO> GetTeacherInfo(string userId)
        {
            Teacher currTeacher = await context.Teachers.FirstOrDefaultAsync(s => s.AppUserId.Equals(userId));
            RegisterTeacherDTO teacher = new RegisterTeacherDTO();
            teacher.UserName = currTeacher.Name;
            teacher.Password = currTeacher.Password;
            teacher.Phone = currTeacher.Phone;
            teacher.Email = currTeacher.Email;
            teacher.CourseID = (int)currTeacher.CourseID;
            return teacher;

        }

        public async Task<Teacher> UpdateTeacher(int id, RegisterTeacherDTO newTeacher)
        {
            Teacher oldTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            ApplicationUser teacherASP = await userManager.FindByEmailAsync(oldTeacher.Email);
            if (oldTeacher != null)
            {
                ////update in aspUsers Table ///
                teacherASP.UserName = newTeacher.UserName;
                teacherASP.PhoneNumber = newTeacher.Phone;
                teacherASP.Email = newTeacher.Email;
                teacherASP.NormalizedEmail = newTeacher.Email.ToUpper();
                teacherASP.NormalizedUserName = newTeacher.UserName.ToUpper();
                await userManager.ChangePasswordAsync(teacherASP, oldTeacher.Password, newTeacher.Password);
                /////update in Teacher Table ////
                oldTeacher.Name = newTeacher.UserName;
                oldTeacher.Phone = newTeacher.Phone;
                oldTeacher.Email = newTeacher.Email;
                oldTeacher.Password = newTeacher.Password;
                oldTeacher.CourseID = newTeacher.CourseID;
                await context.SaveChangesAsync();
                return oldTeacher;
            }
            return null;
            

        }


    }
}


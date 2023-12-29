using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.DB;
using SchoolAPI.DTO;
using SchoolAPI.Model;
using System.Security.Claims;

namespace SchoolAPI.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public StudentRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        } 

        public async Task<Student> CreateStudent(RegisterUserDTO newstudent , string id)
        {
            Student student = new Student();
            student.Name = newstudent.UserName;
            student.Phone = newstudent.Phone;
            student.Email = newstudent.Email;
            student.Year = newstudent.Year;
            student.Password = newstudent.Password;
            student.AppUserId = id;
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();


            return student;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            Student existStudent = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (existStudent != null)
            {
                ApplicationUser studentASP = await userManager.FindByEmailAsync(existStudent.Email);
                context.Students.Remove(existStudent);
                await userManager.DeleteAsync(studentASP);
                await context.SaveChangesAsync();
                return true;

            }  
            return false;

        }

        public async Task<List<Student>> GetAllStudents()
        {
            return ( await context.Students.ToListAsync() );
        }

        public async Task<Student> GetStudentById(int id)
        {
            return (await context.Students.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<RegisterUserDTO> GetStudentInfo(string userId)
        {
            Student currStudent = await context.Students.FirstOrDefaultAsync(s=>s.AppUserId.Equals(userId));
            RegisterUserDTO student = new RegisterUserDTO();
            student.UserName = currStudent.Name;
            student.Password = currStudent.Password;
            student.Phone = currStudent.Phone;
            student.Email = currStudent.Email;
            student.Year = currStudent.Year;
            return student;

        }


        public async Task<Student> UpdateStudent(int id, RegisterUserDTO newStudent )   
        {
            Student oldStudent = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (oldStudent != null)
            {
                ApplicationUser studentAsp = await userManager.FindByEmailAsync(oldStudent.Email);
                ///////Update in AspUersTable///////
                studentAsp.UserName = newStudent.UserName;
                studentAsp.PhoneNumber = newStudent.Phone;
                studentAsp.Email = newStudent.Email;
                studentAsp.NormalizedEmail = newStudent.Email.ToUpper();
                studentAsp.NormalizedUserName = newStudent.UserName.ToUpper();
                await userManager.ChangePasswordAsync(studentAsp , oldStudent.Password, newStudent.Password);
                ////update in student table /////
                oldStudent.Name = newStudent.UserName;
                oldStudent.Phone = newStudent.Phone;
                oldStudent.Email = newStudent.Email;
                oldStudent.Password = newStudent.Password;
                await context.SaveChangesAsync();
                return oldStudent;
            }
            return null;

        }
        
        public async Task<string> Enroll( string Userid , string courseName )
        {
            Student currStudent = await context.Students.Include(s=>s.courses).FirstOrDefaultAsync(x => x.AppUserId.Equals(Userid));
            List<Course> courses = context.Courses.ToList();
            if (currStudent != null)
            {
               foreach( var item in courses)
                {
                    if( courseName.Equals(item.Name) )
                    {
                        if (currStudent.Year != item.Year)
                            return "You Cannot Enroll In This Course";

                        var courseCheck = currStudent.courses.FirstOrDefault(c => c.Name.Equals(courseName));

                        if (courseCheck != null)
                            return "Course Is Already Enrolled";
                        //todo check if course already enrolled
                        currStudent.courses.Add(item);
                        //include students test
                      // Course currCourse = context.Courses.Include(c=>c.Students).FirstOrDefault( c=>c.Name.Equals(item.Name));
                     //  currCourse.Students.Add(currStudent);
                        await context.SaveChangesAsync();
                        return "Couese Is Enrolled Successfully";
                    }
                }
            }
            return "Course Name Is not correct";
        }

        public async Task<string> Disenroll(string Userid , string courseName)
        {

            Student currStudent = await context.Students.Include(s => s.courses).FirstOrDefaultAsync(x => x.AppUserId.Equals(Userid));
            List<Course> courses = context.Courses.ToList();
            if (currStudent != null)
            {
                foreach (var item in courses)
                {
                    if (courseName.Equals(item.Name))
                    {

                        //todo check if course already enrolled
                        currStudent.courses.Remove(item);
                        //include students test
                        // Course currCourse = context.Courses.Include(c=>c.Students).FirstOrDefault( c=>c.Name.Equals(item.Name));
                        //  currCourse.Students.Add(currStudent);
                        await context.SaveChangesAsync();
                        return "course Is Disenrolled";
                    }
                }
            }
            return "Course Not Found ";

        }


        public async Task<List<CourseListDTO>> GetCourseList(string userId)
        {
            Student student = await context.Students.Include(s=>s.courses).FirstOrDefaultAsync(s => s.AppUserId.Equals(userId));
            List<CourseListDTO> courseList = new List<CourseListDTO>();
            if(student != null)
            {
                foreach(var item in student.courses)
                {
                    courseList.Add(new CourseListDTO()
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Year = item.Year,
                    });
                }
                return(courseList);
            }
            return null;
        }
       
    }
}

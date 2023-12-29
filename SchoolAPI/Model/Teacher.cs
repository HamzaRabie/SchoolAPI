using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AppUserId { get; set; }
        public string Phone { get; set; }

        [ForeignKey("Course")]
        public int ?CourseID { get; set; }
        public Course ?Course { get; set;}
    }
}

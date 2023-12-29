namespace SchoolAPI.DTO
{
    public class RegisterTeacherDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CourseID { get; set; }
    }
}

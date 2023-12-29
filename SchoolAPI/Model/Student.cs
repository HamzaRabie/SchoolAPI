namespace SchoolAPI.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password {  get; set; }
        public string Year { get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }
        public string AppUserId { get; set; }
        public List<Course>courses { get; set; }
    }
}

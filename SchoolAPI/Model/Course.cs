namespace SchoolAPI.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public List<Student> Students { get; set; }//=new List<Student>();
        public List<Teacher> Teachers { get; set; } //=new List<Teacher>();
    }
}

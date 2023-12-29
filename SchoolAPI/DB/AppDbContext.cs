using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Model;

namespace SchoolAPI.DB
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet <Teacher> Teachers { get; set; }

        public AppDbContext( DbContextOptions options ) : base( options )
        {
            
        }

        
    }
}

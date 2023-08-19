using Microsoft.EntityFrameworkCore;
using StudentCourseApi.Models;

namespace StudentCourseApi
{
    public class StudentCourseContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("data source=127.0.0.1,1405;initial catalog=StudentMvc;persist security info=true;user id=SA;password=Sql@2022!;multipleactiveresultsets=true;app=entityframework");
        
        }
    }
}

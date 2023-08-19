using StudentCourseApi.Models;
using StudentCourseApi.StudentCourseCRUD;

namespace StudentCourseApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StudentCourseContext>();

            builder.Services.AddScoped<IRepository<Course>, StCourseRepo<Course>>();
            builder.Services.AddScoped<IRepository<Student>, StCourseRepo<Student>>();
            builder.Services.AddScoped<IRepository<Enrollment>, StCourseRepo<Enrollment>>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
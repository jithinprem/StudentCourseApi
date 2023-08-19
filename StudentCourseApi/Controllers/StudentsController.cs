using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCourseApi.StudentCourseCRUD;

namespace StudentCourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {

        private readonly IRepository<Student> repository;

        public StudentsController(IRepository<Student> repository)
        {
            this.repository = repository;
        }

        


        [HttpGet]
        [Route("AllStudents")]
        public async Task<IActionResult> GetStudentsAsync()
        {
            try {
                var listOfStudent = await repository.GetAllAsync();
                return Ok(listOfStudent);
            }catch (Exception ex)
            {
                return NotFound(ex);
            }
            

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddStudentAsync(CreateStudentDto studentDto)
        {
            //mynote : when new course comes the previous is removed from the cache, new course is added
            try {
                Student student = new Student { StudentName = studentDto.StudentName };
                await repository.AddAsync(student);
                CacheModel<Student>.Delete("Student");
                CacheModel<Student>.Set("Student", student);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
            
        }





        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteStudentAsync(int id)
        {
            //mynote : you can delete the course and also delete from the cache
            // one way might be to check if the cache has the value to be deleted but that could be expensive
            try
            {
                var student = await repository.GetByIdAsync(id);
                if (student == null)
                    return NotFound();
                await repository.DeleteAsync(student);
                CacheModel<Student>.Delete("Student");
                return Ok();
            }
            catch { 
                return NotFound();
            }
            
        }



        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetStudentAsync(int id)
        {
            //mynote: you can check with the cache if it has the course with the id, if yes return it
            var cachedStudent = CacheModel<Student>.Get("Course");

            if (cachedStudent != null)
            {
                if (cachedStudent.StudentId == id)
                {
                    return Ok(cachedStudent);
                }
            }

            try
            {
                var student = await repository.GetByIdAsync(id);
                if (student != null)
                    return Ok(student);
            }
            catch { 
                return NotFound();
            }

            return NotFound();


        }



        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateStudentAsync(Student student)
        {
            //mynote : when we are updating we are removing it from cache and adding the updated value

            try
            {
                await repository.UpdateAsync(student);
                CacheModel<Student>.Delete("Student");
                CacheModel<Student>.Set("Student", student);
                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}

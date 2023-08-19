using Microsoft.AspNetCore.Mvc;
using StudentCourseApi.Models;
using StudentCourseApi.StudentCourseCRUD;

namespace StudentCourseApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : Controller
    {

        private readonly IRepository<Enrollment> repository;

        public EnrollmentController(IRepository<Enrollment> repository)
        {
            this.repository = repository;
        }




        [HttpGet]
        [Route("AllStudents")]
        public async Task<IActionResult> GetEnrollmentAsync()
        {
            try
            {
                var listOfEnrollment = await repository.GetAllAsync();
                return Ok(listOfEnrollment);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddEnrollmentAsync(EnrollmentDto enrollmentDto)
        {
            //mynote : when new course comes the previous is removed from the cache, new course is added
            try
            {
                Enrollment enrollment = new Enrollment { CourseId = enrollmentDto.CourseId, StudentId = enrollmentDto.StudentId };
                await repository.AddAsync(enrollment);
                CacheModel<Enrollment>.Delete("Enrollment");
                CacheModel<Enrollment>.Set("Enrollment", enrollment);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }





        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteEnrollmentAsync(int id)
        {
            //mynote : you can delete the course and also delete from the cache
            // one way might be to check if the cache has the value to be deleted but that could be expensive
            try
            {
                var enrollment = await repository.GetByIdAsync(id);
                if (enrollment == null)
                    return NotFound();
                await repository.DeleteAsync(enrollment);
                CacheModel<Enrollment>.Delete("Enrollment");
                return Ok();
            }
            catch
            {
                return NotFound();
            }

        }



        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetStudentAsync(int id)
        {
            //mynote: you can check with the cache if it has the course with the id, if yes return it
            var cachedEnrollment = CacheModel<Enrollment>.Get("Course");

            if (cachedEnrollment != null)
            {
                if (cachedEnrollment.EnrollId == id)
                {
                    return Ok(cachedEnrollment);
                }
            }

            try
            {
                var enrollment = await repository.GetByIdAsync(id);
                if (enrollment != null)
                    return Ok(enrollment);
            }
            catch
            {
                return NotFound();
            }

            return NotFound();


        }



        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateStudentAsync(Enrollment enrollment)
        {
            //mynote : when we are updating we are removing it from cache and adding the updated value

            try
            {
                await repository.UpdateAsync(enrollment);
                CacheModel<Enrollment>.Delete("Student");
                CacheModel<Enrollment>.Set("Student", enrollment);
                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}


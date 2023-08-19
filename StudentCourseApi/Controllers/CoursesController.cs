using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudentCourseApi.Models;
using StudentCourseApi.StudentCourseCRUD;

namespace StudentCourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : Controller
    {
        private readonly IRepository<Course> repository;

        public CoursesController(IRepository<Course> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("AllCoursesSync")]
        public ActionResult GetCourses()
        {
            try {
                var listOfCourse = repository.GetAll();
                return Ok(listOfCourse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return NotFound();
            

        }


        [HttpGet]
        [Route("AllCourses")]
        public async Task<IActionResult> GetCoursesAsync()
        {

            var listOfCourse = await repository.GetAllAsync();
            foreach (var course in listOfCourse)
            {
                Console.WriteLine($"{course.CourseName}");
            }
            return Ok(listOfCourse);

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddCourseAsync(CreateCourseDto courseDto)
        {
            //mynote : when new course comes the previous is removed from the cache, new course is added

            Course course = new Course { CourseName = courseDto.Name, CourseDescription = courseDto.Description };
            await repository.AddAsync(course);
            CacheModel<Course>.Delete("Course");
            CacheModel<Course>.Set("Course", course);

            return Ok();
        }





        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteCourseAsync(int id)
        {
            //mynote : you can delete the course and also delete from the cache
            // one way might be to check if the cache has the value to be deleted but that could be expensive

            var course = await repository.GetByIdAsync(id);
            if (course == null)
                return NotFound();
            await repository.DeleteAsync(course);
            CacheModel<Course>.Delete("Course");
            return Ok();
        }



        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetCourseAsync(int id)
        {
            //mynote: you can check with the cache if it has the course with the id, if yes return it
            var cachedCourse = CacheModel<Course>.Get("Course");

            if (cachedCourse != null)
            {
                if (cachedCourse.CourseId == id)
                {
                    return Ok(cachedCourse);
                }
            }

            var course = await repository.GetByIdAsync(id);
            if (course != null)
                return Ok(course);

            return NotFound();


        }



        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateCourseAsync(Course course)
        {
            //mynote : when we are updating we are removing it from cache and adding the updated value

            try
            {
                await repository.UpdateAsync(course);
                CacheModel<Course>.Delete("Course");
                CacheModel<Course>.Set("Course", course);
                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }

}

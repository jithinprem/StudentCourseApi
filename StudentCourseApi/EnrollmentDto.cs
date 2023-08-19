using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCourseApi
{
    public class EnrollmentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentCourseApi
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }

    }
}

using System.Collections.Generic;

namespace CQRS.Models
{
    public sealed class StudentCoursesDto
    {
        public StudentCoursesDto()
        {
            Courses = new List<CourseViewDTO>();
        }

        public StudentsViewDto student { get; set; }
        public List<CourseViewDTO> Courses { get; set; }
    }

    public sealed class CourseViewDTO
    {
        public string CourseName { get; set; }
    }
}
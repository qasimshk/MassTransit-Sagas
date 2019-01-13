using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CQRS.Entities
{
    public class Enrolled
    {
        public Enrolled()
        {
            students = new Student();
            Courses = new Course();
        }

        [Key]
        public int EnrollId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student students { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Courses { get; set; }
    }
}
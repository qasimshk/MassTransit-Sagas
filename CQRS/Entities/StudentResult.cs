using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CQRS.Entities
{
    public enum Grades
    {
        Distinction,
        Merit,
        Pass,
        Fail
    }

    public class StudentResult
    {
        public StudentResult()
        {
            student = new Student();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int studentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student student { get; set; }

        [Required]
        public Grades Grade { get; set; }
    }
}
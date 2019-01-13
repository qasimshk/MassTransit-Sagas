using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CQRS.Entities
{
    public class Course
    {
        public Course()
        {
            Enrolleds = new List<Enrolled>();
        }

        [Key]
        public int CourseId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual IList<Enrolled> Enrolleds { get; set; }
        
    }
}
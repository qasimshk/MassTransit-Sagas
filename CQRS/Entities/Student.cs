using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CQRS.Entities
{
    public class Student
    {
        public Student()
        {
            Enrolleds = new List<Enrolled>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public virtual IList<Enrolled> Enrolleds { get; set; }
    }
}
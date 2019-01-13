using System;

namespace CQRS.Models
{
    public class PatchStudentDto
    {        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
    }
}

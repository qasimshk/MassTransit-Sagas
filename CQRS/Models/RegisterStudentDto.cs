using System;

namespace CQRS.Models
{
    public sealed class RegisterStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
    }
}
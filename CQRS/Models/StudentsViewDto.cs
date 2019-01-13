using System;

namespace CQRS.Models
{
    public sealed class StudentsViewDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
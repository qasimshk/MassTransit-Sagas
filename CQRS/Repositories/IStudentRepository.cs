using CQRS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Repositories
{
    public interface IStudentRepository : IDisposable
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<IEnumerable<Student>> GetStudentFilterByIdAsync(int studentId);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<Student> GetStudentByEmailAsync(string studentEmail);
        Task<Student> GetCoursesByStudent(int studentId);
        Task CreateStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(Student student);
    }
}
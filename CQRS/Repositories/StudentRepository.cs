using CQRS.Business.Repository;
using CQRS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Repositories
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository, IDisposable
    {
        private readonly SchoolContext _schoolContext;

        public StudentRepository(SchoolContext schoolContext) : base(schoolContext)
        {
            _schoolContext = schoolContext;
        }

        public async Task<Student> GetCoursesByStudent(int studentId)
        {
            var enroll = await _schoolContext.Students
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Enrolleds)
                .ThenInclude(c => c.Courses)                
                .FirstOrDefaultAsync();
            return enroll;
        }

        public async Task CreateStudentAsync(Student student)
        {
            await Create(student);
            await CommitAsync();
        }

        public async Task DeleteStudentAsync(Student student)
        {
            Delete(student);
            await CommitAsync();
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            var student = await FindAllAsync();
            return student.OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();
        }

        public async Task<Student> GetStudentByEmailAsync(string studentEmail)
        {            
            var student = await FindByConditionAync(o => o.Email.Equals(studentEmail));
            return student.DefaultIfEmpty(null)
                    .FirstOrDefault();
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            var student = await FindByConditionAync(o => o.StudentId.Equals(studentId));
            return student.DefaultIfEmpty(null)
                    .FirstOrDefault();
        }

        public async Task<IEnumerable<Student>> GetStudentFilterByIdAsync(int studentId)
        {
            var student = await FindByConditionAync(o => o.StudentId.Equals(studentId));
            return student.DefaultIfEmpty(null)
                    .ToList();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            Update(student);
            await CommitAsync();
        }
    }
}
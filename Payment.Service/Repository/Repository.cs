using Payment.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Service.Business
{
    public class Repository : IRepository
    {
        public async Task<Student> GetStudentByID(int studentId)
        {
            return await Task.Run(() => GetStudents()
                .FirstOrDefault(s => s.StudentId.Equals(studentId)));
        }

        private IReadOnlyList<Student> GetStudents()
        {
            return new List<Student> {
                new Student {
                    StudentId = 1,
                    StudentName = "Donald Duck"
                },
                new Student {
                    StudentId = 2,
                    StudentName = "Mickey Mouse"
                }
            };
        }
    }
}

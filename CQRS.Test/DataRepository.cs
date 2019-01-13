using AutoMapper;
using CQRS.Entities;
using CQRS.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Test
{
    public class DataRepository
    {
        public IMapper GetAutoMapperInstance()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return mockMapper.CreateMapper();
        }

        public Mock<IStudentRepository> GetMock()
        {
            var repository = new Mock<IStudentRepository>();

            // Read
            repository.Setup(x => x.GetStudentFilterByIdAsync(It.IsAny<int>()))
               .Returns((int studentId) =>
               {
                   return Task.Run(() => GetAllStudents()
                    .Where(x => x.StudentId.Equals(studentId))
                    .DefaultIfEmpty(null)
                    .ToList()
                    .Cast<Student>());
               });

            repository.Setup(x => x.GetAllStudentsAsync()).Returns(Task.Run(() => GetAllStudents()));

            repository.Setup(x => x.GetStudentByEmailAsync(It.IsAny<string>()))
                .Returns((string studentEmail) =>
                {
                    return Task.Run(() => GetAllStudents()
                    .FirstOrDefault(x => x.Email.Contains(studentEmail)));
                });

            repository.Setup(x => x.GetStudentByIdAsync(It.IsAny<int>()))
                .Returns((int studentId) =>
                {
                    return Task.Run(() => GetAllStudents()
                     .FirstOrDefault(x => x.StudentId.Equals(studentId)));
                });

            repository.Setup(x => x.GetCoursesByStudent(It.IsAny<int>()))
                .Returns((int studentId) =>
                {
                    return Task.Run(() => GetStudentWithCourse(studentId));
                });

            // Add
            repository.Setup(x => x.CreateStudentAsync(It.IsAny<Student>())).Returns(
                (Student target) => { return StudentCommand(target); });

            // Update
            repository.Setup(x => x.UpdateStudentAsync(It.IsAny<Student>())).Returns(
                (Student target) => { return StudentCommand(target); });

            // Delete
            repository.Setup(x => x.DeleteStudentAsync(It.IsAny<Student>())).Returns(
                (Student target) => { return StudentCommand(target); });

            return repository;
        }

        private Student GetStudentWithCourse(int studentId)
        {
            var resp = GetAllStudents()
                .FirstOrDefault(x => x.StudentId.Equals(studentId));

            resp.Enrolleds = GetAllEnrolleds()
                .Where(x => x.StudentId == studentId).ToList();
            return resp;
        }

        private Task<bool> StudentCommand(Student student)
        {
            return Task.Run(() => true);
        }

        private IEnumerable<Student> GetAllStudents()
        {
            return new List<Student> {
                new Student {
                    StudentId = 1,
                    FirstName = "Donald",
                    LastName = "Duckies",
                    Email = "mm@abc.com",
                    DOB = DateTime.Parse("1984-11-30")
                },
                new Student {
                    StudentId = 2,
                    FirstName = "Mickey",
                    LastName = "Mice",
                    Email = "MickeyMouse@abc.com",
                    DOB = DateTime.Parse("1984-11-30")
                }
            };
        }

        private IEnumerable<Course> GetAllCourses()
        {
            return new List<Course> {
                new Course {
                    CourseId = 1,
                    Name = "Chemistry"
                },
                new Course {
                    CourseId = 2,
                    Name = "Maths"
                },
                new Course {
                    CourseId = 3,
                    Name = "Physics"
                }
            };
        }

        private IEnumerable<Enrolled> GetAllEnrolleds()
        {
            return new List<Enrolled> {
                new Enrolled {
                    EnrollId = 1,
                    StudentId = 1,
                    CourseId = 1
                },
                new Enrolled {
                    EnrollId = 2,
                    StudentId = 1,
                    CourseId = 2
                },
                new Enrolled {
                    EnrollId = 3,
                    StudentId = 1,
                    CourseId = 3
                }
            };
        }
    }
}
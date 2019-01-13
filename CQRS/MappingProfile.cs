using AutoMapper;
using CQRS.Commands;
using CQRS.Entities;
using CQRS.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq;

namespace CQRS
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source is request
            // Destination is the converting entity or object

            CreateMap<Student, StudentsViewDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.StudentId))
                .ForMember(
                dest => dest.FullName,
                opts => opts.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Age,
                opts => opts.MapFrom(src => src.DOB.CalculateAge()));

            CreateMap<RegisterStudentDto, Student>().ReverseMap();

            CreateMap<UpdateStudentDto, Student>()
                .ForMember(
                dest => dest.DOB,
                opts => opts.Condition(src => (src.DOB > DateTime.MinValue)))
                .ForMember(
                dest => dest.FirstName,
                opt => opt.Condition(src => (!string.IsNullOrEmpty(src.FirstName))))
                .ForMember(
                dest => dest.LastName,
                opt => opt.Condition(src => (!string.IsNullOrEmpty(src.LastName))));

            CreateMap<Student, PatchStudentDto>().ReverseMap();

            CreateMap<Student, StudentCoursesDto>()
               //Student mapping
               .ForPath(
                dest => dest.student.Id,
                opt => opt.MapFrom(src => src.StudentId))
               .ForPath(
                dest => dest.student.FullName,
                opts => opts.MapFrom(src => src.FirstName + " " + src.LastName))
               .ForPath(dest => dest.student.Age,
                opts => opts.MapFrom(src => src.DOB.CalculateAge()))
               .ForPath(dest => dest.student.Email,
                opts => opts.MapFrom(src => src.Email))

               //Course mapping
               .ForMember(
                dest => dest.Courses,
                opt => opt.MapFrom(src => src.Enrolleds
                    .Select(lb => new CourseViewDTO { CourseName = lb.Courses.Name })));

            //CQRS
            CreateMap<UpdateStudentDto, EditStudentInfoCommand>();
            CreateMap<RegisterStudentDto, RegisterStudentInfoCommand>();
            CreateMap<RegisterStudentInfoCommand, Student>();
            CreateMap<JsonPatchDocument<PatchStudentDto>, JsonPatchDocument<PartialEditStudentInfoCommand>>();
            CreateMap<PartialEditStudentInfoCommand, Student>();

            CreateMap<EditStudentInfoCommand, Student>()
                .ForMember(
                dest => dest.DOB,
                opts => opts.Condition(src => (src.DOB > DateTime.MinValue)))
                .ForMember(
                dest => dest.FirstName,
                opt => opt.Condition(src => (!string.IsNullOrEmpty(src.FirstName))))
                .ForMember(
                dest => dest.LastName,
                opt => opt.Condition(src => (!string.IsNullOrEmpty(src.LastName))));
        }
    }
}
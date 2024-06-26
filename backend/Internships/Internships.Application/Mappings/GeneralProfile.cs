using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Features.Companies.Commands.CreateCompany;
using Internships.Core.Features.Companies.Queries.GetAllCompanies;
using Internships.Core.Features.Departments.Commands.CreateDepartment;
using Internships.Core.Features.Departments.Queries.GetAllDepartments;
using Internships.Core.Features.Employees.Commands.CreateEmployee;
using Internships.Core.Features.Employees.Queries.GetAllEmployees;
using Internships.Core.Features.Employees.Queries.GetEmployeesByCompanyId;
using Internships.Core.Features.Internships.Commands.CreateInternship;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
using Internships.Core.Features.Internships.Queries.GetInternshipsByUserId;
using Internships.Core.Features.InternshipStatuses.Commands.CreateInternshipStatus;
using Internships.Core.Features.Statuses.Commands.CreateStatus;
using System.Linq;

namespace Internships.Core.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Company, GetAllCompaniesViewModel>().ReverseMap();
            CreateMap<CreateCompanyCommand, Company>();
            CreateMap<GetAllCompaniesQuery, GetAllCompaniesParameter>();

            CreateMap<Employee, GetAllEmployeesViewModel>()
                .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<GetAllEmployeesQuery, GetAllEmployeesParameter>();
            CreateMap<GetEmployeesByCompanyIdQuery, GetAllEmployeesParameter>();

            CreateMap<Department, GetAllDepartmentsViewModel>().ReverseMap();
            CreateMap<CreateDepartmentCommand, Department>();
            CreateMap<GetAllDepartmentsQuery, GetAllDepartmentsParameter>();

            CreateMap<Internship, GetAllInternshipsViewModel>()
                .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.UserTcKimlikNo, opt => opt.MapFrom(src => src.User.TcKimlikNo))
                .ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => src.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name))
                .ForMember(dest => dest.Comment,
                opt => opt.MapFrom(src => src.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Comment));
            CreateMap<CreateInternshipCommand, Internship>();
            CreateMap<GetAllInternshipsQuery, GetAllInternshipsParameter>();

            CreateMap<Internship, GetInternshipsByUserIdViewModel>()
                .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => src.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name))
                .ForMember(dest => dest.Comment,
                opt => opt.MapFrom(src => src.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Comment));
            CreateMap<GetInternshipsByUserIdQuery, GetAllInternshipsParameter>();

            CreateMap<CreateStatusCommand, Status>();
            CreateMap<CreateInternshipStatusCommand, InternshipStatus>();
        }
    }
}
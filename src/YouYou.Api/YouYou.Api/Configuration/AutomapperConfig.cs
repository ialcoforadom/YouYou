using AutoMapper;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Api.ViewModels.Addresses;
using YouYou.Api.ViewModels.BackOfficeUsers;
using YouYou.Api.ViewModels.BankData;
using YouYou.Api.ViewModels.Clients;
using YouYou.Api.ViewModels.Employees;
using YouYou.Api.ViewModels.Genders;
using YouYou.Api.ViewModels.Users;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;
using YouYou.Business.Utils;

namespace YouYou.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {

            CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom<TranslateResolver, string>(src => src.Name))
                .ReverseMap();

            #region FilterPagination

            CreateMap<PaginationFilterBase, PaginationFilterBase>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(src => src.PageNumber < 1 ? 1 : src.PageNumber))
                .ForMember(dest => dest.PageSize, src => src.MapFrom(src => src.PageSize > 50 ? 50 : src.PageSize));

            CreateMap<BackOfficeUsersFilter, BackOfficeUsersFilter>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(src => src.PageNumber < 1 ? 1 : src.PageNumber))
                .ForMember(dest => dest.PageSize, src => src.MapFrom(src => src.PageSize > 50 ? 50 : src.PageSize));

            CreateMap<EmployeeFilter, EmployeeFilter>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(src => src.PageNumber < 1 ? 1 : src.PageNumber))
                .ForMember(dest => dest.PageSize, src => src.MapFrom(src => src.PageSize > 50 ? 50 : src.PageSize));

            CreateMap<ClientFilter, ClientFilter>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(src => src.PageNumber < 1 ? 1 : src.PageNumber))
                .ForMember(dest => dest.PageSize, src => src.MapFrom(src => src.PageSize > 50 ? 50 : src.PageSize));
            #endregion

            #region BackOfficeUser

            CreateMap<BackOfficeUserCreateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF))).ReverseMap();

            CreateMap<BackOfficeUserUpdateViewModel, BackOfficeUser>().ReverseMap()
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(c => c.User.PhysicalPerson.Birthday))
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.User.PhysicalPerson.Gender))
                .ForMember(dest => dest.RoleId, src => src.MapFrom(c => c.User.UserRoles.FirstOrDefault().RoleId));

            CreateMap<BackOfficeUserUpdateViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(c => c.Email))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.Email))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<BackOfficeUserUpdateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF)))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.Gender));

            CreateMap<BackOfficeUserUpdateViewModel, Gender>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<BackOfficeUser, BackOfficeUserListViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom<TranslateResolver, string>(src => src.User.UserRoles.FirstOrDefault().Role.Name))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(c => c.User.PhysicalPerson.Birthday))
                .ForMember(dest => dest.GenderName, src => src.MapFrom(c => c.User.PhysicalPerson.Gender.TypeGender.Name))
                .ForMember(dest => dest.Disabled, src => src.MapFrom(c => c.User.Disabled));

            CreateMap<BackOfficeUser, BackOfficeUserCreateViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.RoleId, src => src.MapFrom(c => c.User.UserRoles.FirstOrDefault().Role.Id));

            #endregion

            #region Employee

            CreateMap<EmployeeCreateViewModel, Employee>();

            CreateMap<EmployeeCreateViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(c => UsefulFunctions.RemoveNonNumeric(c.CPF)))
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(c => c.Password));

            CreateMap<EmployeeCreateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF))).ReverseMap()
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.Gender));

            CreateMap<Employee, EmployeeListViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(dd => dd.User.PhysicalPerson.Name))
                .ForMember(dest => dest.NickName, src => src.MapFrom(dd => dd.User.NickName))
                .ForMember(dest => dest.City, src => src.MapFrom(dd => dd.Address.City.Name))
                .ForMember(dest => dest.RolesNames, src => src.MapFrom(dd => dd.User.UserRoles.Select(r => r.Role.Name)))
                .ForMember(dest => dest.GenderName, src => src.MapFrom(dd => dd.User.PhysicalPerson.Gender.TypeGender.Name))
                .ForMember(dest => dest.Disabled, src => src.MapFrom(dd => dd.User.Disabled))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(dd => dd.User.PhysicalPerson.Birthday));

            CreateMap<EmployeeUpdateViewModel, Employee>().ReverseMap()
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.NickName, src => src.MapFrom(c => c.User.NickName))
                .ForMember(dest => dest.Address, src => src.MapFrom(c => c.Address))
                .ForMember(dest => dest.BankData, src => src.MapFrom(c => c.BankData))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(c => c.User.PhysicalPerson.Birthday))
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.User.PhysicalPerson.Gender))
                .ForMember(dest => dest.RolesId, src => src.MapFrom(c => c.User.UserRoles.Select(c => c.RoleId).ToList()));

            CreateMap<EmployeeUpdateViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF)))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<EmployeeUpdateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF)))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.Gender));

            #endregion

            #region Address
            CreateMap<AddressViewModel, Address>()
                .ForMember(dest => dest.CEP, src => src.MapFrom(c =>
                UsefulFunctions.RemoveNonNumeric(c.CEP)));

            CreateMap<Address, AddressViewModel>()
                .ForMember(dest => dest.StateId, src => src.MapFrom(c => c.City.StateId));
            #endregion

            #region BankData
            CreateMap<BankDataViewModel, BankData>()
                .ForMember(dest => dest.CpfOrCnpjHolder, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpjHolder))).ReverseMap();
            #endregion

            #region Gender
            CreateMap<GenderViewModel, Gender>();
            CreateMap<GenderViewModel, Gender>().ReverseMap();
            #endregion

            #region Client
            CreateMap<ClientCreateViewModel, Client>();

            CreateMap<ClientCreateViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(c => UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpj)))
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(c => c.Password));

            CreateMap<ClientCreateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpj))).ReverseMap()
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.Gender));

            CreateMap<ClientCreateViewModel, JuridicalPerson>()
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(c => c.Name))
                .ForMember(dest => dest.CNPJ, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpj))).ReverseMap();

            CreateMap<Client, ClientListViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(dd => dd.User.IsCompany ? dd.User.JuridicalPerson.CompanyName : dd.User.PhysicalPerson.Name))
                .ForMember(dest => dest.NickName, src => src.MapFrom(dd => dd.User.NickName))
                .ForMember(dest => dest.City, src => src.MapFrom(dd => dd.Address.City.Name))
                .ForMember(dest => dest.RolesNames, src => src.MapFrom(dd => dd.User.UserRoles.Select(r => r.Role.Name)))
                .ForMember(dest => dest.GenderName, src => src.MapFrom(dd => dd.User.PhysicalPerson.Gender.TypeGender.Name))
                .ForMember(dest => dest.Disabled, src => src.MapFrom(dd => dd.User.Disabled))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(dd => dd.User.PhysicalPerson.Birthday));

            CreateMap<ClientUpdateViewModel, Client>().ReverseMap()
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.CpfOrCnpj, src => src.MapFrom(c => c.User.IsCompany ? c.User.JuridicalPerson.CNPJ : c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.IsCompany ? c.User.JuridicalPerson.CompanyName : c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.Address, src => src.MapFrom(c => c.Address))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(c => c.User.PhysicalPerson.Birthday))
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.User.PhysicalPerson.Gender))
                .ForMember(dest => dest.RolesId, src => src.MapFrom(c => c.User.UserRoles.Select(c => c.RoleId).ToList()));

            CreateMap<ClientUpdateViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpj)))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ClientUpdateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpj)))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.Gender));

            CreateMap<ClientUpdateViewModel, JuridicalPerson>()
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(c => c.Name))
                .ForMember(dest => dest.CNPJ, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CpfOrCnpj)))
                .ForMember(x => x.Id, opt => opt.Ignore());
            #endregion
        }

        public class TranslateResolver : IMemberValueResolver<object, object, string, string>
        {
            private readonly IStringLocalizer<SharedResource> _localizer;
            public TranslateResolver(IStringLocalizer<SharedResource> localizer)
            {
                _localizer = localizer;
            }

            public string Resolve(object source, object destination, string sourceMember, string destMember,
                ResolutionContext context)
            {
                return _localizer[sourceMember];
            }
        }
    }
}

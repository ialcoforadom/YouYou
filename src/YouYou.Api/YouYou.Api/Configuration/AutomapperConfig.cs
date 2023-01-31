using AutoMapper;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Api.ViewModels.BackOfficeUsers;
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
            #endregion

            #region BackOfficeUser

            CreateMap<BackOfficeUserCreateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF))).ReverseMap();

            CreateMap<BackOfficeUserUpdateViewModel, PhysicalPerson>()
                .ForMember(dest => dest.CPF, src => src.MapFrom(c =>
                    UsefulFunctions.RemoveNonNumeric(c.CPF)))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<BackOfficeUserUpdateViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(c => c.Email))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.Email))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<BackOfficeUser, BackOfficeUserUpdateViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.RoleId, src => src.MapFrom(c => c.User.UserRoles.FirstOrDefault().RoleId));

            CreateMap<BackOfficeUser, BackOfficeUserListViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom<TranslateResolver, string>(src => src.User.UserRoles.FirstOrDefault().Role.Name))
                .ForMember(dest => dest.Disabled, src => src.MapFrom(c => c.User.Disabled));

            CreateMap<BackOfficeUser, BackOfficeUserCreateViewModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.User.PhysicalPerson.Name))
                .ForMember(dest => dest.CPF, src => src.MapFrom(c => c.User.PhysicalPerson.CPF))
                .ForMember(dest => dest.Email, src => src.MapFrom(c => c.User.Email))
                .ForMember(dest => dest.RoleId, src => src.MapFrom(c => c.User.UserRoles.FirstOrDefault().Role.Id));

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

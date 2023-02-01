using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using YouYou.Api.Helpers;
using YouYou.Api.ViewModels.BackOfficeUsers;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.BackOfficeUsers;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;

namespace YouYou.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("{culture:culture}/api/[controller]")]
    public class BackOfficeUsersController : MainController<BackOfficeUsersController>
    {
        private readonly IMapper _mapper;
        private readonly IBackOfficeUserService _backOfficeUserService;
        private readonly IUserService _userService;
        private readonly IUriService _uriService;

        public BackOfficeUsersController(IErrorNotifier errorNotifier,
            IUser user, IStringLocalizer<BackOfficeUsersController> localizer,
            IMapper mapper, IBackOfficeUserService backOfficeUserService,
            IUserService userService, IUriService uriService)
            : base(errorNotifier, user, localizer)
        {
            _mapper = mapper;
            _backOfficeUserService = backOfficeUserService;
            _userService = userService;
            _uriService = uriService;
        }
        /// <summary>
        /// Endpoint para criar um novo usuário do tipo 'Admin ou Operador'.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Add(BackOfficeUserCreateViewModel userViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var backOfficeUser = new BackOfficeUser(userViewModel.Name, userViewModel.CPF, userViewModel.Email, userViewModel.Birthday, userViewModel.Gender.TypeGenderId, userViewModel.Gender.Description);

            await _backOfficeUserService.Add(backOfficeUser, userViewModel.Password, userViewModel.RoleId);
            return CustomResponse(userViewModel);
        }
        /// <summary>
        /// Endpoint para listar todos os usários do tipo 'Admin ou Operador'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BackOfficeUsersFilter filter)
        {
            string route = GetRouteWithFilters(filter);

            var validFilter = _mapper.Map<BackOfficeUsersFilter>(filter);
            var backOfficeUsers = await _backOfficeUserService.GetAllWithIncludes(validFilter);
            var backOfficeUsersDto = _mapper.Map<List<BackOfficeUserListViewModel>>(backOfficeUsers);

            int totalRecords = await _backOfficeUserService.GetTotalRecords(validFilter);
            var pagedReponse = PaginationHelper.CreatePagedReponse<BackOfficeUserListViewModel>
                (backOfficeUsersDto, validFilter, totalRecords, _uriService, route);

            return Ok(pagedReponse);
        }

        private string GetRouteWithFilters(BackOfficeUsersFilter filter)
        {
            string route = Request.Path.Value;
            if (!string.IsNullOrEmpty(filter.Name))
            {
                route = QueryHelpers.AddQueryString(route, "name", filter.Name);
            }
            if (!string.IsNullOrEmpty(filter.CPF))
            {
                route = QueryHelpers.AddQueryString(route, "cpf", filter.CPF);
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                route = QueryHelpers.AddQueryString(route, "email", filter.Email);
            }
            if (!string.IsNullOrEmpty(filter.Role))
            {
                route = QueryHelpers.AddQueryString(route, "role", filter.Role);
            }

            return route;
        }
        /// <summary>
        /// Endpoint para buscar um usário pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BackOfficeUserUpdateViewModel>> GetById(Guid id)
        {
            var backOfficeUser = await _backOfficeUserService.GetByIdWithIncludes(id);
            if (backOfficeUser == null) return NotFound();

            return _mapper.Map<BackOfficeUserUpdateViewModel>(backOfficeUser);
        }
        /// <summary>
        /// Endpoint para editar um usuário do tipo 'Admin ou Operador'
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<BackOfficeUserUpdateViewModel>> Update(BackOfficeUserUpdateViewModel userViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var backOfficeUser = await _backOfficeUserService.GetByIdWithIncludesTracked(userViewModel.Id);

            MappingUpdate(userViewModel, backOfficeUser);

            await _backOfficeUserService.Update(backOfficeUser, userViewModel.Password, userViewModel.RoleId);
            return CustomResponse(userViewModel);
        }
        private void MappingUpdate(BackOfficeUserUpdateViewModel userViewModel, BackOfficeUser backOfficeUser)
        {
            _mapper.Map<BackOfficeUserUpdateViewModel, ApplicationUser>
                (userViewModel, backOfficeUser.User);
            _mapper.Map<BackOfficeUserUpdateViewModel, PhysicalPerson>
                (userViewModel, backOfficeUser.User.PhysicalPerson);
        }
        /// <summary>
        /// Endpoint para deletar logicamente um usuário do tipo 'Admin ou Operador'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            await _backOfficeUserService.Remove(id);

            return CustomResponse();
        }
        /// <summary>
        /// Endpoint para desativar um usuário do tipo 'Admin ou Operador'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Disable/{id:Guid}")]
        public async Task<ActionResult> Disable(Guid id)
        {
            var backOfficeUser = await _backOfficeUserService.GetById(id);
            if (backOfficeUser == null) return NotFound();

            await _backOfficeUserService.Disable(backOfficeUser.UserId);

            return CustomResponse();
        }
        /// <summary>
        /// Endpoint para ativar um usuário do tipo 'Admin ou Operador'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Enable/{id:Guid}")]
        public async Task<ActionResult> Enable(Guid id)
        {
            var backOfficeUser = await _backOfficeUserService.GetById(id);
            if (backOfficeUser == null) return NotFound();

            await _backOfficeUserService.Enable(backOfficeUser.UserId);

            return CustomResponse();
        }
    }
}

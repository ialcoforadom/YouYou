using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using YouYou.Api.Helpers;
using YouYou.Api.ViewModels.Employees;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Employees;
using YouYou.Business.Models;
using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Pagination;

namespace YouYou.Api.Controllers
{
    [Authorize]
    [Route("{culture:culture}/api/[controller]")]
    public class EmployeesController : MainController<EmployeesController>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IUriService _uriService;

        public EmployeesController(IErrorNotifier errorNotifier,
            IUser appUser, IStringLocalizer<EmployeesController> localizer,
            IMapper mapper, IEmployeeService employeeService,
            IUriService uriService) : base(errorNotifier, appUser, localizer)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _uriService = uriService;
        }
        /// <summary>
        /// Endpoint para criação de funcionários
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador, Coordenador")]
        [HttpPost]
        public async Task<ActionResult> Add(EmployeeCreateViewModel userViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            EmployeeDto employee = Mapping(userViewModel);

            await _employeeService.Add(employee, userViewModel.RoleId);

            return CustomResponse(userViewModel);
        }

        private EmployeeDto Mapping(EmployeeCreateViewModel employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            employee.User = _mapper.Map<ApplicationUser>(employeeVM);
            employee.User.PhysicalPerson = _mapper.Map<PhysicalPerson>(employeeVM);

            return new EmployeeDto(employee, employeeVM.Phones, employeeVM.Password);
        }

        [Authorize(Roles = "Admin, Operador, Coordenador")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EmployeeFilter filter)
        {
            string route = GetRouteWithFilters(filter);

            var validFilter = _mapper.Map<EmployeeFilter>(filter);
            var employees = await _employeeService.GetAllWithIncludes(validFilter);
            var employeeDto = _mapper.Map<List<EmployeeListViewModel>>(employees);

            int totalRecords = await _employeeService.GetTotalRecords(validFilter);
            var pagedReponse = PaginationHelper.CreatePagedReponse<EmployeeListViewModel>
                (employeeDto, validFilter, totalRecords, _uriService, route);

            return Ok(pagedReponse);
        }

        private string GetRouteWithFilters(EmployeeFilter filter)
        {
            string route = Request.Path.Value;
            if (!string.IsNullOrEmpty(filter.Name))
            {
                route = QueryHelpers.AddQueryString(route, "name", filter.Name);
            }
            if (!string.IsNullOrEmpty(filter.NickName))
            {
                route = QueryHelpers.AddQueryString(route, "nickName", filter.NickName);
            }
            if (!string.IsNullOrEmpty(filter.Plate))
            {
                route = QueryHelpers.AddQueryString(route, "plate", filter.Plate);
            }
            if (!string.IsNullOrEmpty(filter.City))
            {
                route = QueryHelpers.AddQueryString(route, "city", filter.City);
            }

            return route;
        }
    }
}

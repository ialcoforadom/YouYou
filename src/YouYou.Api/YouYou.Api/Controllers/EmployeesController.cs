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
        /// <summary>
        /// Endpoint para listar todos os funcionários
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint para deletar logicamente o funcionário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador, Coordenador")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee == null) return NotFound();

            await _employeeService.Remove(employee);
            return CustomResponse();
        }

        [Authorize(Roles = "Admin, Operador")]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<EmployeeUpdateViewModel>> GetById(Guid id)
        {
            var employeeDto = await _employeeService.GetDtoByIdWithIncludes(id);
            if (employeeDto == null) return NotFound();

            var result = _mapper.Map<EmployeeUpdateViewModel>(employeeDto.Employee);
            result.Phones = employeeDto.Phones;

            return result;
        }

        [Authorize(Roles = "Admin, Operador")]
        [HttpPut("Disable/{id:Guid}")]
        public async Task<ActionResult> Disable(Guid id)
        {
            var directDeliverer = await _employeeService.GetById(id);
            if (directDeliverer == null) return NotFound();

            await _employeeService.Disable(directDeliverer.UserId);
            return CustomResponse();
        }

        [Authorize(Roles = "Admin, Operador")]
        [HttpPut("Enable/{id:Guid}")]
        public async Task<ActionResult> Enable(Guid id)
        {
            var directDeliverer = await _employeeService.GetById(id);
            if (directDeliverer == null) return NotFound();

            await _employeeService.Enable(directDeliverer.UserId);
            return CustomResponse();
        }

        [Authorize(Roles = "Admin, Operador")]
        [HttpPut]
        public async Task<ActionResult<EmployeeUpdateViewModel>> Update(EmployeeUpdateViewModel userViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var employee = await _employeeService.GetByIdWithIncludesTracked(userViewModel.Id);
            MappingUpdate(userViewModel, employee);

            var employeeDto = new EmployeeDto(employee, userViewModel.Phones, userViewModel.Password);
            await _employeeService.Update(employeeDto);

            return CustomResponse(userViewModel);
        }

        private void MappingUpdate(EmployeeUpdateViewModel userViewModel, Employee employee)
        {
            _mapper.Map<EmployeeUpdateViewModel, Employee>
                (userViewModel, employee);
            _mapper.Map<EmployeeUpdateViewModel, ApplicationUser>
                (userViewModel, employee.User);
            _mapper.Map<EmployeeUpdateViewModel, PhysicalPerson>
                (userViewModel, employee.User.PhysicalPerson);
        }
    }
}

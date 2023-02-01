using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Employees;
using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;
using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Pagination;
using YouYou.Business.Models.Validations;
using YouYou.Business.Services;

namespace YouYou.Business.Services
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUser _appUser;
        private readonly IUserService _userService;
        private readonly IExtraPhoneService _extraPhoneService;

        public EmployeeService(IErrorNotifier errorNotifier,
            IEmployeeRepository employeeRepository,
            IUserService userService, IUser appUser,
            IExtraPhoneService extraPhoneService) : base(errorNotifier)
        {
            _employeeRepository = employeeRepository;
            _userService = userService;
            _appUser = appUser;
            _extraPhoneService = extraPhoneService;
        }

        public async Task Add(EmployeeDto employeeDto, Guid roleId)
        {
            var employee = employeeDto.Employee;

            if (!ExecuteValidation(new AddressValidation(), employee.Address) ||
               !ExecuteValidation(new PhysicalPersonValidation(), employee.User.PhysicalPerson) ||
               !ExecuteValidation(new BankDataValidation(), employee.BankData)) return;

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _extraPhoneService.AddPhones(employee.User, employeeDto.Phones.ToList());

                var succeeded = await _userService.Add(employee.User, employeeDto.Password);
                if (succeeded)
                {
                    var roleSucceeded = await _userService.AddRole(employee.User, roleId);
                    if (roleSucceeded)
                    {
                        await _employeeRepository.Add(employee);
                        tr.Complete();
                    }
                }
            }
        }
        public async Task<IEnumerable<Employee>> GetAllWithIncludes(EmployeeFilter filter)
        {
            return await _employeeRepository.GetAllWithIncludes(filter);
        }
        public async Task<int> GetTotalRecords(EmployeeFilter filter)
        {
            return await _employeeRepository.GetTotalRecords(filter);
        }
        public void Dispose()
        {
            _employeeRepository?.Dispose();
        }
    }
}

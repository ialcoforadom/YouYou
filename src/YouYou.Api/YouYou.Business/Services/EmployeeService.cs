using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.BankDatas;
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
        private readonly IBankDataService _bankDataService;

        public EmployeeService(IErrorNotifier errorNotifier,
            IEmployeeRepository employeeRepository,
            IUserService userService, IUser appUser,
            IExtraPhoneService extraPhoneService, IBankDataService bankDataService) : base(errorNotifier)
        {
            _employeeRepository = employeeRepository;
            _userService = userService;
            _appUser = appUser;
            _extraPhoneService = extraPhoneService;
            _bankDataService = bankDataService;
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
        public async Task<Employee> GetById(Guid id)
        {
            return await _employeeRepository.GetById(id);
        }
        public async Task Remove(Employee employee)
        {
            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool succeeded = await _userService.Remove(employee.UserId);
                if (succeeded)
                {
                    await _employeeRepository.Remove(employee.Id);
                    tr.Complete();
                }
            }
        }
        public async Task<EmployeeDto> GetDtoByIdWithIncludes(Guid id)
        {
            var directDeliverer = await _employeeRepository.GetByIdWithIncludes(id);
            var phones = _extraPhoneService.MapperPhones(directDeliverer.User);

            return new EmployeeDto(directDeliverer, phones);
        }
        public async Task Disable(Guid userId)
        {
            await _userService.Disable(userId);
        }
        public async Task Enable(Guid userId)
        {
            await _userService.Enable(userId);
        }
        public async Task<Employee> GetByIdWithIncludesTracked(Guid id)
        {
            return await _employeeRepository.GetByIdWithIncludesTracked(id);
        }
        public async Task Update(EmployeeDto employeeDto)
        {
            var directDeliverer = employeeDto.Employee;

            if (!ExecuteValidation(new AddressValidation(), directDeliverer.Address) ||
                !ExecuteValidation(new BankDataValidation(), directDeliverer.BankData) ||
                !ExecuteValidation(new PhysicalPersonValidation(), directDeliverer.User.PhysicalPerson)) return;

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _bankDataService.SetCpfOrCnpjHolderInBankData(directDeliverer.BankData, directDeliverer.User.PhysicalPerson.CPF);
                await _extraPhoneService.UpdatePhones(directDeliverer.User, employeeDto.Phones.ToList());

                bool succeeded;

                if (string.IsNullOrEmpty(employeeDto.Password))
                {
                    succeeded = await _userService.Update(directDeliverer.User);
                }
                else
                {
                    succeeded = await _userService.Update(directDeliverer.User, employeeDto.Password);
                }

                if (succeeded)
                {
                    await _employeeRepository.Update(directDeliverer);
                    tr.Complete();
                }
            }
        }
        public void Dispose()
        {
            _employeeRepository?.Dispose();
        }
    }
}

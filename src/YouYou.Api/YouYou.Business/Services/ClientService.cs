using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.BankDatas;
using YouYou.Business.Interfaces.Clients;
using YouYou.Business.Interfaces.Employees;
using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;
using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Enums;
using YouYou.Business.Models.Pagination;
using YouYou.Business.Models.Validations;
using YouYou.Business.Services;

namespace YouYou.Business.Services
{
    public class ClientService : BaseService, IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUser _appUser;
        private readonly IUserService _userService;
        private readonly IExtraPhoneService _extraPhoneService;
        private readonly IBankDataService _bankDataService;

        public ClientService(IErrorNotifier errorNotifier,
            IClientRepository clientRepository,
            IUserService userService, IUser appUser,
            IExtraPhoneService extraPhoneService, IBankDataService bankDataService) : base(errorNotifier)
        {
            _clientRepository = clientRepository;
            _userService = userService;
            _appUser = appUser;
            _extraPhoneService = extraPhoneService;
            _bankDataService = bankDataService;
        }

        public async Task Add(ClientDto clientDto, TypeClientEnum type, IFormFile file)
        {
            var client = clientDto.Client;
            switch (type)
            {
                case TypeClientEnum.PhysicalPerson:
                    if (!ExecuteValidation(new AddressValidation(), client.Address) || !ExecuteValidation(new PhysicalPersonValidation(), client.User.PhysicalPerson)) return;
                    break;
                case TypeClientEnum.JuridicalPerson:
                    if (!ExecuteValidation(new AddressValidation(), client.Address) || !ExecuteValidation(new JuridicalPersonValidation(), client.User.JuridicalPerson)) return;
                    client.User.IsCompany = true;
                    break;
            }

            if (file != null)
            {
                client.User.FileName = file.FileName;

                using (var target = new MemoryStream())
                {
                    await file.CopyToAsync(target);
                    client.User.DataFiles = target.ToArray();
                }
            }

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _extraPhoneService.AddPhones(client.User, clientDto.Phones.ToList());

                var succeeded = await _userService.Add(client.User, clientDto.Password);
                if (succeeded)
                {
                    var roleSucceeded = await _userService.AddRoles(client.User, clientDto.Roles);
                    if (roleSucceeded)
                    {
                        await _clientRepository.Add(client);
                        tr.Complete();
                    }
                }
            }
        }
        public async Task<IEnumerable<Client>> GetAllWithIncludes(ClientFilter filter)
        {
            return await _clientRepository.GetAllWithIncludes(filter);
        }

        public async Task<int> GetTotalRecords(ClientFilter filter)
        {
            return await _clientRepository.GetTotalRecords(filter);
        }
        public async Task<Client> GetById(Guid id)
        {
            return await _clientRepository.GetById(id);
        }
        public async Task Remove(Client client)
        {
            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool succeeded = await _userService.Remove(client.UserId);
                if (succeeded)
                {
                    await _clientRepository.Remove(client.Id);
                    tr.Complete();
                }
            }
        }
        public async Task<ClientDto> GetDtoByIdWithIncludes(Guid id)
        {
            var client = await _clientRepository.GetByIdWithIncludes(id);
            var phones = _extraPhoneService.MapperPhones(client.User);

            return new ClientDto(client, phones);
        }
        public async Task Disable(Guid userId)
        {
            await _userService.Disable(userId);
        }
        public async Task Enable(Guid userId)
        {
            await _userService.Enable(userId);
        }
        public async Task<Client> GetByIdWithIncludesTracked(Guid id)
        {
            return await _clientRepository.GetByIdWithIncludesTracked(id);
        }
        public async Task Update(ClientDto clientDto, TypeClientEnum type, IFormFile file)
        {
            var client = clientDto.Client;
            switch (type)
            {
                case TypeClientEnum.PhysicalPerson:
                    if (!ExecuteValidation(new AddressValidation(), client.Address) || !ExecuteValidation(new PhysicalPersonValidation(), client.User.PhysicalPerson)) return;
                    break;
                case TypeClientEnum.JuridicalPerson:
                    if (!ExecuteValidation(new AddressValidation(), client.Address) || !ExecuteValidation(new JuridicalPersonValidation(), client.User.JuridicalPerson)) return;
                    break;
            }

            if (file != null)
            {
                client.User.FileName = file.FileName;

                using (var target = new MemoryStream())
                {
                    await file.CopyToAsync(target);
                    client.User.DataFiles = target.ToArray();
                }
            }

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _extraPhoneService.UpdatePhones(client.User, clientDto.Phones.ToList());

                bool succeeded;

                if (string.IsNullOrEmpty(clientDto.Password))
                    succeeded = await _userService.Update(client.User);
                else
                    succeeded = await _userService.Update(client.User, clientDto.Password);

                if (succeeded)
                {
                    var roleValidation = await _userService.UpdateRoles(client.User, clientDto.Roles.ToList());
                    if (roleValidation)
                    {
                        await _clientRepository.Update(client);
                        tr.Complete();
                    }
                }
            }
        }
        public void Dispose()
        {
            _clientRepository?.Dispose();
        }
    }
}

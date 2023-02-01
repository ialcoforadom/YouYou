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

        public async Task Add(ClientDto clientDto, Guid roleId, TypeClientEnum type)
        {
            var client = clientDto.Client;
            switch (type)
            {
                case TypeClientEnum.PhysicalPerson:
                    if (!ExecuteValidation(new AddressValidation(), client.Address) ||
                       !ExecuteValidation(new PhysicalPersonValidation(), client.User.PhysicalPerson)) return;
                    break;
                case TypeClientEnum.JuridicalPerson:
                    if (!ExecuteValidation(new AddressValidation(), client.Address) ||
                       !ExecuteValidation(new JuridicalPersonValidation(), client.User.JuridicalPerson)) return;
                    break;
            }
            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _extraPhoneService.AddPhones(client.User, clientDto.Phones.ToList());

                var succeeded = await _userService.Add(client.User, clientDto.Password);
                if (succeeded)
                {
                    var roleSucceeded = await _userService.AddRole(client.User, roleId);
                    if (roleSucceeded)
                    {
                        await _clientRepository.Add(client);
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

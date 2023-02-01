using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Api.ViewModels.Clients;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Clients;
using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Models;
using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Enums;

namespace YouYou.Api.Controllers
{
    [Authorize(Roles = "Admin, Operador")]
    [Route("{culture:culture}/api/[controller]")]
    public class ClientsController : MainController<ClientsController>
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly IExtraPhoneService _extraPhoneService;

        public ClientsController(IErrorNotifier errorNotifier, IUser appUser,
            IStringLocalizer<ClientsController> localizer,
            IMapper mapper,
            IClientService clientService, 
            IExtraPhoneService extraPhoneService)
            : base(errorNotifier, appUser, localizer)
        {
            _mapper = mapper;
            _clientService = clientService;
            _extraPhoneService = extraPhoneService;
        }

        [HttpPost]
        public async Task<ActionResult> Add(ClientCreateViewModel userViewModel, [Required]TypeClientEnum type)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var clientDto = Mapping(userViewModel, type);

            //ManageTranshipmentsController.AddUserTranshipmentIdentity(client, userViewModel.CPF, userViewModel.Email);

            //client.User.PhysicalPerson = _mapper.Map<PhysicalPerson>(userViewModel);

            await _clientService.Add(clientDto, userViewModel.RoleId, type);

            return CustomResponse(userViewModel);

            //using var context = Db;
            //using (var transaction = context.Database.BeginTransaction())
            //{
            //    _extraPhoneService.AddPhones(client.User, userViewModel.Phones.ToList());

            //    var succeeded = await _transhipmentPhysicalPersonService.Add(client, userViewModel.Password);
            //    if (succeeded)
            //    {
            //        transaction.Commit();
            //    }
            //}

            return CustomResponse(userViewModel);
        }
        private ClientDto Mapping(ClientCreateViewModel clientVM, TypeClientEnum type)
        {
            var client = _mapper.Map<Client>(clientVM);
            client.User = _mapper.Map<ApplicationUser>(clientVM);
            switch (type)
            {
                case TypeClientEnum.PhysicalPerson:
                    client.User.PhysicalPerson = _mapper.Map<PhysicalPerson>(clientVM);
                    break;
                case TypeClientEnum.JuridicalPerson:
                    client.User.JuridicalPerson = _mapper.Map<JuridicalPerson>(clientVM);
                    break;
            }

            return new ClientDto(client, clientVM.Phones, clientVM.Password);
        }
    }
}

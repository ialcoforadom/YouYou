using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Clients;
using YouYou.Business.Interfaces.ExtraPhones;

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

        //[HttpPost]
        //public async Task<ActionResult> Add(TranshipmentPhysicalPersonCreateViewModel userViewModel)
        //{
        //    if (!ModelState.IsValid) return CustomResponse(ModelState);

        //    var transhipment = _mapper.Map<Transhipment>(userViewModel);

        //    ManageTranshipmentsController.AddUserTranshipmentIdentity(transhipment, userViewModel.CPF, userViewModel.Email);

        //    transhipment.User.PhysicalPerson = _mapper.Map<PhysicalPerson>(userViewModel);

        //    using var context = Db;
        //    using (var transaction = context.Database.BeginTransaction())
        //    {
        //        _extraPhoneService.AddPhones(transhipment.User, userViewModel.Phones.ToList());

        //        var succeeded = await _transhipmentPhysicalPersonService.Add(transhipment, userViewModel.Password);
        //        if (succeeded)
        //        {
        //            transaction.Commit();
        //        }
        //    }

        //    return CustomResponse(userViewModel);
        //}
    }
}

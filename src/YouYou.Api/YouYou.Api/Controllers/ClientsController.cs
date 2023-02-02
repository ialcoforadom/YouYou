using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Api.Helpers;
using YouYou.Api.ViewModels.Clients;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Clients;
using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Models;
using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Enums;
using YouYou.Business.Models.Pagination;

namespace YouYou.Api.Controllers
{
    [Authorize(Roles = "Admin, Operador")]
    [Route("{culture:culture}/api/[controller]")]
    public class ClientsController : MainController<ClientsController>
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly IUriService _uriService;

        public ClientsController(IErrorNotifier errorNotifier, IUser appUser,
            IStringLocalizer<ClientsController> localizer,
            IMapper mapper,
            IClientService clientService, IUriService uriService)
            : base(errorNotifier, appUser, localizer)
        {
            _mapper = mapper;
            _clientService = clientService;
            _uriService = uriService;
        }
        /// <summary>
        /// Endpoint para criar um novo cliente.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Add(
            [ModelBinder(BinderType = typeof(JsonModelBinder))] ClientCreateViewModel userViewModel,
            IFormFile? file, [Required] TypeClientEnum type)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var clientDto = Mapping(userViewModel, type);

            await _clientService.Add(clientDto, type, file);

            return CustomResponse(userViewModel);
        }
        private ClientDto Mapping(ClientCreateViewModel clientVM, TypeClientEnum type)
        {
            var client = _mapper.Map<Client>(clientVM);
            client.User = _mapper.Map<ApplicationUser>(clientVM);
            switch (type)
            {
                case TypeClientEnum.PhysicalPerson: client.User.PhysicalPerson = _mapper.Map<PhysicalPerson>(clientVM); break;
                case TypeClientEnum.JuridicalPerson: client.User.JuridicalPerson = _mapper.Map<JuridicalPerson>(clientVM); break;
            }

            return new ClientDto(client, clientVM.Phones, clientVM.Password, clientVM.RolesId);
        }
        /// <summary>
        /// Endpoint que retorna a lista de todos os clientes cadastrados.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ClientFilter filter)
        {
            string route = Request.Path.Value;

            var validFilter = _mapper.Map<ClientFilter>(filter);
            var transhipments = await _clientService.GetAllWithIncludes(validFilter);
            var transhipmentsDto = _mapper.Map<List<ClientListViewModel>>(transhipments);

            int totalRecords = await _clientService.GetTotalRecords(validFilter);
            var pagedReponse = PaginationHelper.CreatePagedReponse<ClientListViewModel>
                (transhipmentsDto, validFilter, totalRecords, _uriService, route);

            return Ok(pagedReponse);
        }
        /// <summary>
        /// Endpoint que deletar um cliente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador, Coordenador")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            var employee = await _clientService.GetById(id);
            if (employee == null) return NotFound();

            await _clientService.Remove(employee);
            return CustomResponse();
        }
        /// <summary>
        /// Endpoint que busca um cliente a partir do Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador")]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ClientUpdateViewModel>> GetById(Guid id)
        {
            var clientDto = await _clientService.GetDtoByIdWithIncludes(id);
            if (clientDto == null) return NotFound();

            var result = _mapper.Map<ClientUpdateViewModel>(clientDto.Client);
            result.Phones = clientDto.Phones;

            return result;
        }
        /// <summary>
        /// Endpoint que desabilita um cliente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador")]
        [HttpPut("Disable/{id:Guid}")]
        public async Task<ActionResult> Disable(Guid id)
        {
            var client = await _clientService.GetById(id);
            if (client == null) return NotFound();

            await _clientService.Disable(client.UserId);
            return CustomResponse();
        }
        /// <summary>
        /// Endpoint que habilita um client.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador")]
        [HttpPut("Enable/{id:Guid}")]
        public async Task<ActionResult> Enable(Guid id)
        {
            var client = await _clientService.GetById(id);
            if (client == null) return NotFound();

            await _clientService.Enable(client.UserId);
            return CustomResponse();
        }
        /// <summary>
        /// Endpoint que edita um cliente.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Operador")]
        [HttpPut]
        public async Task<ActionResult<ClientUpdateViewModel>> Update(
            [ModelBinder(BinderType = typeof(JsonModelBinder))] ClientUpdateViewModel userViewModel,
            IFormFile? file, [Required] TypeClientEnum type)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var client = await _clientService.GetByIdWithIncludesTracked(userViewModel.Id);
            var clientDto = MappingUpdate(userViewModel, client, type);

            await _clientService.Update(clientDto, type, file);

            return CustomResponse(userViewModel);
        }

        private ClientDto MappingUpdate(ClientUpdateViewModel userViewModel, Client client, TypeClientEnum type)
        {
            _mapper.Map<ClientUpdateViewModel, Client>
                (userViewModel, client);
            _mapper.Map<ClientUpdateViewModel, ApplicationUser>
                (userViewModel, client.User);
            switch (type)
            {
                case TypeClientEnum.PhysicalPerson:
                    _mapper.Map<ClientUpdateViewModel, PhysicalPerson> (userViewModel, client.User.PhysicalPerson); break;
                case TypeClientEnum.JuridicalPerson:
                    _mapper.Map<ClientUpdateViewModel, JuridicalPerson>(userViewModel, client.User.JuridicalPerson); break;
            }
            return new ClientDto(client, userViewModel.Phones, userViewModel.Password, userViewModel.RolesId);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using YouYou.Api.Models;

namespace YouYou.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {

        public UsuarioController()
        {
        }

        [HttpGet]
        public Usuario Get() 
        {
            return new Usuario()
            {
                Id = new Guid(),
                Name = "teste",
                Cpf = "testecpf",
                Email = "teste@teste.com",
                Password = "senha teste",
                IsActived = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };
        }
    };
}
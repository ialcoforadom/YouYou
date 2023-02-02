using System;
using System.Collections.Generic;
using System.Text;

namespace YouYou.Business.Models.Dtos
{
    public class ClientDto
    {
        public ClientDto(Client client, ICollection<string> phones, string password, ICollection<Guid> roles)
        {
            this.Client = client;
            Phones = phones;
            Password = password;
            Roles = roles;
        }
        public ClientDto(Client client, ICollection<string> phones)
        {
            this.Client = client;
            Phones = phones;
        }

        public Client Client { get; set; }

        public ICollection<string> Phones { get; set; }
        public ICollection<Guid> Roles { get; set; }

        public string Password { get; set; }
    }
}

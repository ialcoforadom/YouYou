using System;
using System.Collections.Generic;
using System.Text;

namespace YouYou.Business.Models.Dtos
{
    public class EmployeeDto
    {
        public EmployeeDto(Employee employee, ICollection<string> phones, string password)
        {
            this.Employee = employee;
            Phones = phones;
            Password = password;
        }
        public EmployeeDto(Employee employee, ICollection<string> phones)
        {
            this.Employee = employee;
            Phones = phones;
        }

        public Employee Employee { get; set; }

        public ICollection<string> Phones { get; set; }

        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;

namespace YouYou.Business.Interfaces.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllWithIncludes(EmployeeFilter filter);
        Task<int> GetTotalRecords(EmployeeFilter filter);
    }
}

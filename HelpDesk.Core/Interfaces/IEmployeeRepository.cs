using HelpDesk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IReadOnlyList<Employee>> GetActiveAsync();
    }
}

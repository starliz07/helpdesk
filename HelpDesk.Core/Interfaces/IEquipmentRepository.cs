using HelpDesk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<Equipment?> GetByIdAsync(int id);
        Task<IReadOnlyList<Equipment>> GetActiveAsync();
    }
}

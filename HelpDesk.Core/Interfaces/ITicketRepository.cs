using HelpDesk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int id);
        Task<int> CreateAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task<IReadOnlyList<Ticket>> GetAllAsync();
        Task<IReadOnlyList<Ticket>> GetByStatusAsync(string status);
        Task<IReadOnlyList<Ticket>> GetOpenByAssigneeAsync(int employeeId);
        Task AddStatusHistoryAsync(int ticketId, string from, string to, int? changedBy);
    }
}

using HelpDesk.Core.Enums;
using HelpDesk.Core.Interfaces;
using HelpDesk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Tests.Unit.Fakes
{
    public class FakeTicketRepository : ITicketRepository
    {
        public List<Ticket> Storage { get; } = new();

        public Task<Ticket?> GetByIdAsync(int id) =>
            Task.FromResult(Storage.FirstOrDefault(t => t.Id == id));

        public Task<int> CreateAsync(Ticket t)
        {
            t.Id = Storage.Count + 1;
            Storage.Add(t);
            return Task.FromResult(t.Id);
        }

        public Task UpdateAsync(Ticket t) => Task.CompletedTask;

        public Task<IReadOnlyList<Ticket>> GetAllAsync() =>
            Task.FromResult<IReadOnlyList<Ticket>>(Storage.ToList());

        public Task<IReadOnlyList<Ticket>> GetByStatusAsync(string status) =>
            Task.FromResult<IReadOnlyList<Ticket>>(
                Storage.Where(t => t.Status.ToString() == status).ToList());

        public Task<IReadOnlyList<Ticket>> GetOpenByAssigneeAsync(int employeeId) =>
            Task.FromResult<IReadOnlyList<Ticket>>(
                Storage.Where(t => t.AssigneeId == employeeId
                                && t.Status != Status.Closed
                                && t.Status != Status.Cancelled).ToList());

        public Task AddStatusHistoryAsync(int ticketId, string from, string to, int? changedBy) =>
            Task.CompletedTask;
    }
}

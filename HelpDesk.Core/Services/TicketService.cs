using HelpDesk.Core.Enums;
using HelpDesk.Core.Exceptions;
using HelpDesk.Core.Interfaces;
using HelpDesk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Services
{
    public class TicketService
    {
        private readonly ITicketRepository _tickets;
        private readonly IEmployeeRepository _employees;
        private readonly StatusTransitionValidator _validator;

        public TicketService(ITicketRepository tickets, IEmployeeRepository employees, StatusTransitionValidator validator)
        {
            _tickets = tickets;
            _employees = employees;
            _validator = validator;
        }

        public async Task<int> CreateTicketAsync(Ticket ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket.Title))
                throw new DomainException("Title cannot be empty.");

            if (ticket.Description.Length > 2000)
                throw new DomainException("Description cannot exceed 2000 characters.");

            if (!Enum.IsDefined(typeof(Priority), ticket.Priority))
                throw new DomainException("Unknown priority.");

            ticket.CreatedAt = DateTimeOffset.UtcNow;
            ticket.Status = Status.New;
            return await _tickets.CreateAsync(ticket);
        }

        public async Task AssignExecutorAsync(int ticketId, int employeeId)
        {
            var ticket = await _tickets.GetByIdAsync(ticketId)
                         ?? throw new NotFoundException($"Ticket {ticketId} not found.");

            _ = await _employees.GetByIdAsync(employeeId)
                ?? throw new NotFoundException($"Employee {employeeId} not found.");

            ticket.AssigneeId = employeeId;
            await _tickets.UpdateAsync(ticket);
        }

        public async Task ChangeStatusAsync(int ticketId, Status newStatus, int changedBy)
        {
            var ticket = await _tickets.GetByIdAsync(ticketId)
                         ?? throw new NotFoundException($"Ticket {ticketId} not found.");

            if (!_validator.IsTransitionAllowed(ticket.Status, newStatus))
                throw new DomainException($"Недопустимый переход: {ticket.Status} → {newStatus}");

            var oldStatus = ticket.Status;
            ticket.Status = newStatus;

            await _tickets.UpdateAsync(ticket);
            await _tickets.AddStatusHistoryAsync(ticketId, oldStatus.ToString(), newStatus.ToString(), changedBy);
        }

        public async Task SoftDeleteEmployeeAsync(int employeeId)
        {
            var openTickets = await _tickets.GetOpenByAssigneeAsync(employeeId);
            if (openTickets.Count > 0)
                throw new DomainException("Cannot delete employee with open tickets.");

            await _employees.SoftDeleteAsync(employeeId);
        }
    }
}

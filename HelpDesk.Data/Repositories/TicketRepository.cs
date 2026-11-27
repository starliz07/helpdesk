using HelpDesk.Core.Interfaces;
using HelpDesk.Core.Models;
using HelpDesk.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace HelpDesk.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ISqlConnectionFactory _factory;

        public TicketRepository(ISqlConnectionFactory factory) => _factory = factory;

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            using var conn = _factory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<Ticket>(
                "SELECT * FROM tickets WHERE id = @Id", new { Id = id });
        }

        public async Task<int> CreateAsync(Ticket t)
        {
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(@"
            INSERT INTO tickets (author_id, equipment_id, assignee_id,
                title, description, priority, status, created_at)
            VALUES (@AuthorId, @EquipmentId, @AssigneeId,
                @Title, @Description, @Priority, @Status, @CreatedAt)
            RETURNING id;",
                new
                {
                    t.AuthorId,
                    t.EquipmentId,
                    t.AssigneeId,
                    t.Title,
                    t.Description,
                    Priority = t.Priority.ToString(),
                    Status = t.Status.ToString(),
                    t.CreatedAt
                });
        }

        public async Task UpdateAsync(Ticket t)
        {
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(@"
            UPDATE tickets
            SET assignee_id = @AssigneeId,
                status = @Status
            WHERE id = @Id",
                new { t.Id, t.AssigneeId, Status = t.Status.ToString() });
        }

        public async Task<IReadOnlyList<Ticket>> GetAllAsync()
        {
            using var conn = _factory.CreateConnection();
            var list = await conn.QueryAsync<Ticket>("SELECT * FROM tickets ORDER BY id");
            return list.AsList();
        }

        public async Task<IReadOnlyList<Ticket>> GetByStatusAsync(string status)
        {
            using var conn = _factory.CreateConnection();
            var list = await conn.QueryAsync<Ticket>(
                "SELECT * FROM tickets WHERE status = @S ORDER BY id",
                new { S = status });
            return list.AsList();
        }

        public async Task<IReadOnlyList<Ticket>> GetOpenByAssigneeAsync(int employeeId)
        {
            using var conn = _factory.CreateConnection();
            var list = await conn.QueryAsync<Ticket>(@"
            SELECT * FROM tickets
            WHERE assignee_id = @Id
              AND status NOT IN ('Closed','Cancelled')",
                new { Id = employeeId });
            return list.AsList();
        }

        public async Task AddStatusHistoryAsync(int ticketId, string from, string to, int? changedBy)
        {
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(@"
            INSERT INTO ticket_status_history
                (ticket_id, from_status, to_status, changed_at, changed_by_id)
            VALUES (@TicketId, @From, @To, now(), @By)",
                new { TicketId = ticketId, From = from, To = to, By = changedBy });
        }
    }
}

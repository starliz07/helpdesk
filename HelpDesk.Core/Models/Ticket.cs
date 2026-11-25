using HelpDesk.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int EquipmentId { get; set; }
        public int? AssigneeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public Status Status { get; set; } = Status.New;
        public DateTimeOffset CreatedAt { get; set; }
    }
}

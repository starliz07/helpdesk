using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}

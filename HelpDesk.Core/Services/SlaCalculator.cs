using HelpDesk.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Services
{
    public class SlaCalculator
    {
        public DateTimeOffset CalculateSlaDeadline(DateTimeOffset createdAt, Priority priority)
        {
            var hours = priority switch
            {
                Priority.Low => 72,
                Priority.Normal => 24,
                Priority.High => 8,
                Priority.Critical => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unknown priority")
            };
            return createdAt.AddHours(hours);
        }
    }
}

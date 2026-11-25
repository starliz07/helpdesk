using HelpDesk.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Services
{
    public class StatusTransitionValidator
    {
        public bool IsTransitionAllowed(Status from, Status to)
        {
            return from switch
            {
                Status.New => to is Status.InProgress or Status.Cancelled,
                Status.InProgress => to is Status.Resolved or Status.Cancelled,
                Status.Resolved => to is Status.Closed,
                Status.Closed => false,
                Status.Cancelled => false,
                _ => false
            };
        }
    }
}

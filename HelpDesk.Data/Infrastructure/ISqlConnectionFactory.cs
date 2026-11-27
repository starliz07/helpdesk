using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HelpDesk.Data.Infrastructure
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

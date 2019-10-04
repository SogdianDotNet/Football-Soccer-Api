using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Common.Base.Configuration
{
    public partial class AppConfiguration
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public partial class ConnectionStrings
    {
        public string AzureSqlDatabase { get; set; }
    }
}

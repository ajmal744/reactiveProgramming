using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.SignalR.Hubs;

namespace AgentsDashboard.Server
{
    public interface IContextHolder
    {
        IHubCallerConnectionContext<dynamic> AgentScansHealthClients { get; set; }  
    }
}

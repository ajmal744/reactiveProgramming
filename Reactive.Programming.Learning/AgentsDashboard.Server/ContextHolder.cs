using Microsoft.AspNet.SignalR.Hubs;

namespace AgentsDashboard.Server
{
    public class ContextHolder : IContextHolder
    {
        public IHubCallerConnectionContext<dynamic> AgentScansHealthClients { get; set; }
    }
}
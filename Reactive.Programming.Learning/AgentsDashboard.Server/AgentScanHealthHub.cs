using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace AgentsDashboard.Server
{
    [HubName("AgentScanHealthHub")]
    public class AgentScanHealthHub : Hub
    {
        private readonly IContextHolder contextHolder;

        public AgentScanHealthHub(IContextHolder contextHolder)
        {
            this.contextHolder = contextHolder;
        }

        [HubMethodName("SubscribeAgentScanHealthStream")]
        public async Task SubscribeAgentScanHealthStream()
        {
            contextHolder.AgentScansHealthClients = Clients;

            await Groups.Add(Context.ConnectionId, "agentHealthScans");
        }

        [HubMethodName("UnsubscribeAgentScanHealthStream")]
        public async Task UnsubscribeAgentScanHealthStream()
        {
            await Groups.Remove(Context.ConnectionId, "agentHealthScans");
        }
    }
}

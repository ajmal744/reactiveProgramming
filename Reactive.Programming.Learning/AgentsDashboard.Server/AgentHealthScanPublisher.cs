using System.Threading.Tasks;

namespace AgentsDashboard.Server
{
    public class AgentHealthScanPublisher : IAgentHealthScanPublisher
    {
        private readonly IContextHolder contextHolder;

        public AgentHealthScanPublisher(IContextHolder contextHolder)
        {
            this.contextHolder = contextHolder;
        }

        public async Task Publish(AgentHealth agentHealth)
        {
            var context = contextHolder.AgentScansHealthClients;

            if (context == null)
            {
                return;
            }

            await context.Group("agentHealthScans").OnAgentHealthUpdated(agentHealth);
        }
    }
}
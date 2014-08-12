using System.Threading.Tasks;

namespace AgentsDashboard.Server
{
    public interface IAgentHealthScanPublisher
    {
        Task Publish(AgentHealth agentHealth);
    }
}
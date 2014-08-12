using System;
using System.Collections.Generic;
using System.Threading;

namespace AgentsDashboard.Server
{
    public class SimulateAgentHealthFeed : IAgentHealthFeed
    {
        private readonly IAgentHealthScanPublisher agentHealthScanPublisher;
        private Timer timer;

        private IList<string> agentNames = new List<string>
        {
            "Agent1", "Agent2", "Agent3", "Agent4" , "Agent5"
        };

        public SimulateAgentHealthFeed(IAgentHealthScanPublisher agentHealthScanPublisher)
        {
            this.agentHealthScanPublisher = agentHealthScanPublisher;
        }

        public void Start()
        {
            timer = new Timer(OnTimerElapsed, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(4) );
        }

        private void OnTimerElapsed(object state)
        {
            agentHealthScanPublisher.Publish(new AgentHealth()
            {
                AgentName = GetRandomAgent(),
                LastMachineScanDateTime = DateTime.UtcNow,
                LastSoftwareScanDateTime = DateTime.UtcNow
            });
        }

        private string GetRandomAgent()
        {
            var agentIndex = new Random().Next(0, 4);
            return agentNames[agentIndex];
        }
    }
}
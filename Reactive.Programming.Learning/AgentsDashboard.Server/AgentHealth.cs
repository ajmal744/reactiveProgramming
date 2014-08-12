using System;

namespace AgentsDashboard.Server
{
    public class AgentHealth
    {
        public string AgentName { get; set; }

        public DateTime LastMachineScanDateTime  { get; set; }

        public DateTime LastSoftwareScanDateTime { get; set; }
    }
}
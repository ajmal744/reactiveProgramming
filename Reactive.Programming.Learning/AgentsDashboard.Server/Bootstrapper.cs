using Autofac;

namespace AgentsDashboard.Server
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AgentHealthScanPublisher>().As<IAgentHealthScanPublisher>();
            builder.RegisterType<ContextHolder>().As<IContextHolder>();
            builder.RegisterType<SimulateAgentHealthFeed>().As<IAgentHealthFeed>();

            return builder.Build();
        }
    }
}

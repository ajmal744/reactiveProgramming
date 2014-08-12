using Autofac;

namespace AgentsDashboard.Server
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            return builder.Build();
        }
    }
}

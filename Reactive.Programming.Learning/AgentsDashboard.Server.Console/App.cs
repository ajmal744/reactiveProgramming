using Autofac;
using Microsoft.Owin.Hosting;

namespace AgentsDashboard.Server.Console
{
    public class App
    {
        public static IContainer Container;

        public static void Main(string[] args)
        {
            Container = new Bootstrapper().Build();

            WebApp.Start("http://localhost:8080");
            Container.Resolve<IAgentHealthFeed>().Start();
        }
    }
}

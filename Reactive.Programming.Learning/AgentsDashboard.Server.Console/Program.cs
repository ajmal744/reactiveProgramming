using Autofac;
using Microsoft.Owin.Hosting;
using System;

namespace AgentsDashboard.Server.Console
{
    public class Program
    {
        public static IContainer Container;

        public static void Main(string[] args)
        {
            Container = new Bootstrapper().Build();

            WebApp.Start("http://localhost:8080");
            Container.Resolve<IAgentHealthFeed>().Start();

            System.Console.WriteLine("Started the server providing the agent scan health data periodically.");
            System.Console.ReadLine();
        }
    }
}

using Az.DevOps.Services;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Search.WebApi;

namespace Az.DevOps.Tools.CLI
{
    [HelpOption]
    [Command(Name = "az-devops-tools", FullName = "A .NET Core global tool to extend Azure DevOps")]
    public static class Program
    {
        public static int Main(string[] args)
        {
            using var services = new ServiceCollection()
                .AddSingleton<App, App>()
                .AddSingleton<GitHttpClient, GitHttpClient>()
                .AddSingleton<SearchHttpClient, SearchHttpClient>()
                .AddSingleton(PhysicalConsole.Singleton)
                .AddSingleton<ITemplateService, TemplateService>()
                .BuildServiceProvider();

            var app = services.GetService<App>();
            return app.Execute(args);
        }
    }
}
using Az.DevOps.Services;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Az.DevOps.Tools.CLI
{
    public class App
    {
        public App(ITemplateService templateService, IConsole console)
        {
            TemplateService = templateService;
            Console = console;
        }

        public IConsole Console { get; }
        protected ITemplateService TemplateService { get; }

        public int Execute(string[] args)
        {
            var app = new CommandLineApplication<App>
            {
                Name = "az-devops-tools"
            };

            app.Conventions
                .UseDefaultConventions();

            app.Description = "Provides extension commands for Azure DevOps.";

            app.Command("templates", _ =>
            {
                _.Description = "Provides commands for templates.";

                _.Command("status", _ =>
                {
                    _.Description = "Retrieves status of each template and each pipeline using templates.";

                    var collectionUrl = _.Option("-c|--collection-url", "Specifies the URL of the Azure DevOps collection.", CommandOptionType.SingleValue, true).IsRequired();
                    var projectName = _.Option("-p|--project-name", "Specifies the project name of the Azure DevOps collection.", CommandOptionType.SingleValue, true).IsRequired();
                    var personalAccessToken = _.Option("-pat|--personal-access-token", "Specifies the personal access token (Permissions : Code - Read).", CommandOptionType.SingleValue, true).IsRequired();
                    var templateRepositoryName = _.Option("-rn|--repository-name", "Specifies the name of the repository that contains templates.", CommandOptionType.SingleValue, true).IsRequired();

                    _.OnExecuteAsync(async t =>
                   {
                       try
                       {
                           Console.ResetColor();
                           Console.Write(Figgle.FiggleFonts.Standard.Render("az-devops-tools"));
                           Console.WriteLine();

                           await TemplateService.DisplayStatusAsync(DevOpsTokenAuthentication.Create(new Uri(collectionUrl.Value()), projectName.Value(), personalAccessToken.Value()), templateRepositoryName.Value());
                           return 0;
                       }
                       catch (TemplateException e)
                       {
                           Console.ForegroundColor = ConsoleColor.Red;
                           Console.WriteLine($"ERROR : {e.Message}");
                           Console.WriteLine($"BASE-ERROR : {e.GetBaseException().Message}");
                           Console.WriteLine(e.StackTrace);
                           return 1;
                       }
                       finally
                       {
                           Console.ResetColor();
                       }
                   });
                });

                _.OnValidationError((e) =>
                {
                    Console.ResetColor();
                    Console.Write(Figgle.FiggleFonts.Standard.Render("az-devops-tools"));
                    Console.WriteLine();

                    _.ShowHelp();
                    return 1;
                });

                _.OnExecute(() =>
                {
                    Console.ResetColor();
                    Console.Write(Figgle.FiggleFonts.Standard.Render("az-devops-tools"));
                    Console.WriteLine();

                    _.ShowHelp();
                    return 1;
                });
            });

            app.OnValidationError((_) =>
            {
                Console.ResetColor();
                Console.Write(Figgle.FiggleFonts.Standard.Render("az-devops-tools"));
                Console.WriteLine();

                app.ShowHelp();
                return 1;
            });

            app.OnExecute(() =>
            {
                Console.ResetColor();
                Console.Write(Figgle.FiggleFonts.Standard.Render("az-devops-tools"));
                Console.WriteLine();

                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }
    }
}
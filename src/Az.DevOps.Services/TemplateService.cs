using Az.DevOps.Services.Models;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Search.WebApi;
using Microsoft.VisualStudio.Services.Search.WebApi.Contracts.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Az.DevOps.Services
{
    public class TemplateService : ITemplateService
    {
        private GitHttpClient _git;
        private SearchHttpClient _search;
        private readonly IConsole _console;

        public TemplateService(IConsole console)
        {
            _console = console;
        }

        private async Task<IDictionary<string, Template>> GetTemplatesAsync(string projectName, string templateRepositoryName)
        {
            var result = new Dictionary<string, Template>();
            var filters = new Dictionary<string, IEnumerable<string>> {
                 { "Project", new string[] { projectName} },
                 { "Repository", new string[] { templateRepositoryName } }
            };
            var search = await _search.FetchCodeSearchResultsAsync(new CodeSearchRequest { SearchText = "ext:.yml", Filters = filters, Top = 1000 }, projectName);
            foreach (var item in search.Results)
            {
                var parts = item.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                Array.Reverse(parts);
                if (!result.ContainsKey(parts[2]))
                {
                    result.Add(parts[2], new Template { Name = parts[2] });
                }
                result[parts[2]].Versions.Add(parts[1]);
            }
            return result;
        }

        private async Task<IList<Pipeline>> GetPipelinesAsync(string projectName, string templateRepositoryName)
        {
            var regex = new Regex($".*/(.*)/(v[0-9]*)/.*.yml@{templateRepositoryName}");
            var result = new List<Pipeline>();
            var search = await _search.FetchCodeSearchResultsAsync(new CodeSearchRequest { SearchText = $"@{templateRepositoryName}", Top = 1000 }, projectName);
            foreach (var item in search.Results)
            {
                var pipeline = new Pipeline
                {
                    Path = item.Path,
                    Repository = item.Repository.Name
                };
                var stream = await _git.GetItemContentAsync(projectName, item.Repository.Name, path: item.Path);
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    var matches = regex.Matches(content);
                    foreach (Match match in matches)
                    {
                        pipeline.References.Add(new TemplateReference
                        {
                            TemplateName = ((Match)match.Captures[0]).Groups[1].Value,
                            Version = ((Match)match.Captures[0]).Groups[2].Value
                        });
                    }
                }
                result.Add(pipeline);
            }
            return result;
        }

        private void DisplayPipelinesStatus(IDictionary<string, Template> templates, IList<Pipeline> pipelines)
        {
            _console.WriteLine();
            _console.WriteLine($"********************");
            _console.WriteLine($"* Pipelines status *");
            _console.WriteLine($"********************");

            foreach (var item in pipelines.OrderBy(_ => _.Repository).ThenBy(_ => _.Name))
            {
                _console.WriteLine();
                _console.ForegroundColor = ConsoleColor.White;
                _console.WriteLine($"Pipeline : '{item.Repository}/{item.Name}'");
                _console.ResetColor();
                foreach (var reference in item.References)
                {
                    _console.Write($"    Template '{reference.TemplateName}' : ");
                    if (templates.ContainsKey(reference.TemplateName))
                    {
                        if (reference.Version == templates[reference.TemplateName].Versions.Last())
                        {
                            _console.ForegroundColor = ConsoleColor.Green;
                            _console.WriteLine($"Up to date !");
                            _console.ResetColor();
                        }
                        else
                        {
                            _console.ForegroundColor = ConsoleColor.Yellow;
                            _console.WriteLine($"Upgrade from {reference.Version} to {templates[reference.TemplateName].Versions.Last()}");
                            _console.ResetColor();
                        }
                    }
                    else
                    {
                        _console.ForegroundColor = ConsoleColor.Red;
                        _console.WriteLine($"Template reference not found, fix pipeline quickly !");
                        _console.ResetColor();
                    }
                }
            }
        }

        public async Task DisplayStatusAsync(IDevOpsAuthentication authentication, string templateRepositoryName)
        {
            try
            {
                var _connection = authentication.GetConnection();

                _git = _connection.GetClient<GitHttpClient>();
                _search = _connection.GetClient<SearchHttpClient>();

                // Get templates with versions
                var templates = await GetTemplatesAsync(authentication.ProjectName, templateRepositoryName);

                // Get pipelines that reference templates
                var pipelines = await GetPipelinesAsync(authentication.ProjectName, templateRepositoryName);

                // Display pipelines status
                DisplayPipelinesStatus(templates, pipelines);

                // Display templates status
                DisplayTemplatesStatus(templates, pipelines);
            }
            catch (Exception ex)
            {
                throw new TemplateException("Unable to display status. See inner exceptions for more details", ex);
            }
        }

        private void DisplayTemplatesStatus(IDictionary<string, Template> templates, IList<Pipeline> pipelines)
        {
            _console.WriteLine();
            _console.WriteLine($"********************");
            _console.WriteLine($"* Templates status *");
            _console.WriteLine($"********************");

            var allReferences = pipelines.SelectMany(_ => _.References);
            foreach (var template in templates.Values)
            {
                _console.WriteLine();
                _console.ForegroundColor = ConsoleColor.White;
                _console.WriteLine($"Template : '{template.Name}'");
                _console.ResetColor();
                var versions = allReferences.Where(_ => _.TemplateName == template.Name).Select(_ => _.Version).Distinct();
                foreach (var version in template.Versions)
                {
                    _console.Write($"    Version '{version}' : ");
                    if (versions.Contains(version))
                    {
                        _console.ForegroundColor = ConsoleColor.Green;
                        _console.WriteLine($"Used by pipelines");
                        _console.ResetColor();
                    }
                    else
                    {
                        _console.ForegroundColor = ConsoleColor.Yellow;
                        _console.WriteLine($"Unused. Can be removed !");
                        _console.ResetColor();
                    }
                }
            }
        }
    }
}
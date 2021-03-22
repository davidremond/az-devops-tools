using Microsoft.VisualStudio.Services.WebApi;
using System;

namespace Az.DevOps.Services
{
    public interface IDevOpsAuthentication
    {
        Uri CollectionUri { get; }
        string ProjectName { get; }

        VssConnection GetConnection();
    }
}
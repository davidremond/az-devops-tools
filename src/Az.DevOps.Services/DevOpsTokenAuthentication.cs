using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;

namespace Az.DevOps.Services
{
    public class DevOpsTokenAuthentication : IDevOpsAuthentication
    {
        public Uri CollectionUri { get; internal set; }
        public string ProjectName { get; internal set; }
        public string PersonalAccessToken { get; internal set; }

        public static DevOpsTokenAuthentication Create(Uri collectionUri, string projectName, string personalAccessToken)
        {
            return new DevOpsTokenAuthentication
            {
                CollectionUri = collectionUri,
                ProjectName = projectName,
                PersonalAccessToken = personalAccessToken
            };
        }

        public VssConnection GetConnection()
        {
            var credentials = new VssBasicCredential(string.Empty, PersonalAccessToken);
            return new VssConnection(CollectionUri, credentials);
        }
    }
}
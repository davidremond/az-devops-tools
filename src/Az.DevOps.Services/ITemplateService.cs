using System.Threading.Tasks;

namespace Az.DevOps.Services
{
    public interface ITemplateService
    {
        Task DisplayStatusAsync(IDevOpsAuthentication authentication, string templateRepositoryName);
    }
}
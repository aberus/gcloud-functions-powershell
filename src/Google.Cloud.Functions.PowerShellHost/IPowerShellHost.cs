using System.Threading;
using System.Threading.Tasks;

namespace Google.Cloud.Functions.PowerShellHost;

public interface IPowerShellHost
{
    Task<HttpResponse> RunScriptAsync(string script, HttpRequest request, CancellationToken cancellationToken);
}
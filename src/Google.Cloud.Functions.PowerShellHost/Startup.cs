using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Google.Cloud.Functions.PowerShellHost;

public class Startup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services) =>
        services
            .AddSingleton<IPowerShellHost, PowerShellHost>()
            .AddSingleton<IHttpRequestReader<HttpRequest>, HttpRequestReader>()
            .AddSingleton<IHttpResponseWriter<HttpResponse>, HttpResponseWriter>();
}
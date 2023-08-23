using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Google.Cloud.Functions.PowerShellHost;

[FunctionsStartup(typeof(Startup))]
public class PowerShellFunction : IHttpFunction
{
    private readonly IPowerShellHost _powerShellHost;
    private readonly IHttpRequestReader<HttpRequest> _requestReader;
    private readonly IHttpResponseWriter<HttpResponse> _responseWriter;
    private readonly ILogger _logger;

    public PowerShellFunction(
        IPowerShellHost powerShellHost,
        IHttpRequestReader<HttpRequest> requestReader,
        IHttpResponseWriter<HttpResponse> responseWriter,
        ILogger<PowerShellFunction> logger)
    {
        _powerShellHost = powerShellHost;
        _requestReader = requestReader;
        _responseWriter = responseWriter;
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        HttpRequest data;
        try
        {
            data = await _requestReader.ReadRequestAsync(context.Request);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var response = await _powerShellHost.RunScriptAsync(File.ReadAllText("function.ps1"), data, context.RequestAborted);
        try
        {
            await _responseWriter.WriteResponseAsync(context.Response, response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return;
        }
    }
}

//public class Test
//{

//  public string ToDebugString<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
//  {
//    return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
//  }
//  public string ToDebugString(System.Collections.IDictionary dictionary)
//  {
//    var keyValuePairs = new List<string>();
//    foreach (DictionaryEntry entry in dictionary)
//    {
//      var key = entry.Key.ToString();
//      var value = entry.Value.ToString();
//      keyValuePairs.Add(key + "=" + value);
//    }
//    return "{" + string.Join(",", keyValuePairs) + "}";
//  }



//  /// <summary>The Metadata flavor header name.</summary>
//  internal const string MetadataFlavor = "Metadata-Flavor";

//  /// <summary>The Metadata header response indicating Google.</summary>
//  internal const string GoogleMetadataHeader = "Google";


//  // Constant strings to avoid duplication below.
//  // IP address instead of name to avoid DNS resolution
//  private const string DefaultMetadataAddress = "169.254.169.254";
//  internal const string DefaultMetadataServerUrl = "http://" + DefaultMetadataAddress;
//  private const string ComputeDefaultProjectIdSuffix = "/computeMetadata/v1/project/project-id";
//  private const string ComputeDefaultMetadataSuffix = "/computeMetadata/v1/?recursive=true";

//  /// <summary>
//  /// The effective Compute Engine default service account email URL.
//  /// This takes account of the GCE_METADATA_HOST environment variable.
//  /// </summary>
//  internal static string EffectiveComputeDefaultProjectIdUrl =>
//      GetEffectiveMetadataUrl(ComputeDefaultProjectIdSuffix, DefaultMetadataServerUrl + ComputeDefaultProjectIdSuffix);

//  internal static string EffectiveComputeMetadataUrl =>
//  GetEffectiveMetadataUrl(ComputeDefaultMetadataSuffix, DefaultMetadataServerUrl + ComputeDefaultMetadataSuffix);

//  /// <summary>
//  /// The effective Compute Engine metadata token server URL (with no path).
//  /// This takes account of the GCE_METADATA_HOST environment variable.
//  /// </summary>
//  internal static string EffectiveMetadataServerUrl => GetEffectiveMetadataUrl(null, DefaultMetadataServerUrl);

//  private static string GetEffectiveMetadataUrl(string suffix, string defaultValue)
//  {
//    string? env = Environment.GetEnvironmentVariable("GCE_METADATA_HOST");
//    return string.IsNullOrEmpty(env) ? defaultValue : "http://" + env + suffix;
//  }

//  //_logger.LogInformation(RuntimeInformation.IsOSPlatform(OSPlatform.Windows).ToString());
//  //_logger.LogInformation(ToDebugString(Environment.GetEnvironmentVariables()));


//  //var googleAppCreds = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
//  //_logger.LogInformation(googleAppCreds);



//  //var httpRequest = new HttpRequestMessage(HttpMethod.Get, EffectiveComputeDefaultProjectIdUrl);
//  //httpRequest.Headers.Add(MetadataFlavor, GoogleMetadataHeader);

//  //var response = await new HttpClient().SendAsync(httpRequest).ConfigureAwait(false);
//  //response.EnsureSuccessStatusCode();
//  //var x = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//  //_logger.LogInformation(x);

//  //httpRequest = new HttpRequestMessage(HttpMethod.Get, EffectiveComputeMetadataUrl);
//  //httpRequest.Headers.Add(MetadataFlavor, GoogleMetadataHeader);

//  //response = await new HttpClient().SendAsync(httpRequest).ConfigureAwait(false);
//  //response.EnsureSuccessStatusCode();
//  //var y = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//  //_logger.LogInformation(y);



//  //var xx = context.Request.Headers.Select(x => (x.Key, Value:x.Value.FirstOrDefault())).ToDictionary(x => x.Key);

//  //var a = context.Request.Headers;
//  //var b = context.Response;
//}
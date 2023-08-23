﻿using Google.Cloud.Functions.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Google.Cloud.Functions.PowerShellHost;

public sealed class HttpRequestReader : IHttpRequestReader<HttpRequest>
{
    public async Task<HttpRequest> ReadRequestAsync(Microsoft.AspNetCore.Http.HttpRequest httpRequest)
    {
        var request = new HttpRequest
        {
            Headers = httpRequest.Headers
                      .SelectMany(kv => kv.Value, (kv, v) => new { kv.Key, Value = v })
                      .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase),
            //.Select(x => (x.Key, Value: x.Value.FirstOrDefault()))
            //.ToDictionary(x => x.Key, x => x.Value ?? "", StringComparer.OrdinalIgnoreCase),
            Query = httpRequest.Query
                      .SelectMany(kv => kv.Value, (kv, v) => new { kv.Key, Value = v })
                      .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase),
            Cookies = httpRequest.Cookies
                      .Select(x => (x.Key, x.Value))
                      .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase),
            Path = httpRequest.Path,
            Protocol = httpRequest.Protocol,
            Method = httpRequest.Method
        };

        //request.Body = await new StreamReader(httpRequest.Body).ReadToEndAsync();

        using (var memoryStream = new MemoryStream())
        {
          await httpRequest.Body.CopyToAsync(memoryStream);
          request.Body = memoryStream.ToArray();
        }

        return request;
    }
}

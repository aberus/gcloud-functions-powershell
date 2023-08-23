using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Google.Cloud.Functions.PowerShellHost;

public sealed class HttpResponseWriter : IHttpResponseWriter<HttpResponse>
{
    public async Task WriteResponseAsync(Microsoft.AspNetCore.Http.HttpResponse httpResponse, HttpResponse functionResponse)
    {
        httpResponse.StatusCode = functionResponse.StatusCode;

        if (functionResponse.Headers is not null)
        {
            foreach (DictionaryEntry header in functionResponse.Headers)
            {
                if (header.Key is string stringKey)
                {
                    httpResponse.Headers[stringKey] = header.Value?.ToString();
                }
            }
        }

        if (functionResponse.ContentType is { } contentType)
        {
            httpResponse.ContentType = contentType;
        }

        if (functionResponse.Body is string { } text)
        {
            if (httpResponse.ContentType is null)
            {
                httpResponse.ContentType = "text/plain";
            }

            await httpResponse.WriteAsync(text);
        }
        else if (functionResponse.Body is byte[] { } bytes)
        {
            await httpResponse.Body.WriteAsync(bytes);
        }
        else if (functionResponse.Body is { } body)
        {
            try
            {
                var responseStream = httpResponse.Body;

                //MemoryStream stream = new MemoryStream();
                //IFormatter formatter = new BinaryFormatter();
                //formatter.Serialize(stream, body!);
                //await stream.CopyToAsync(context.Response.Body, context.RequestAborted);

                JsonSerializerOptions serializerOptions = new()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                };

                if (httpResponse.ContentType is null)
                {
                    httpResponse.ContentType = "application/json";
                }

                string jsonString = JsonSerializer.Serialize(body);

                await JsonSerializer.SerializeAsync(responseStream, body, serializerOptions/*, context.RequestAborted*/);
                await responseStream.FlushAsync(/*context.RequestAborted*/);
            }
            catch (OperationCanceledException) /*when (context.RequestAborted.IsCancellationRequested)*/ { }
        }
    }
}
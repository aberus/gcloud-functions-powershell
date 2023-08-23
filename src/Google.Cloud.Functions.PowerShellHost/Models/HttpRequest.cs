using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Google.Cloud.Functions.PowerShellHost
{
    /// <summary>
    /// Custom type represent the context of the in-coming Http request.
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Gets the Headers of the Http request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets the Query of the Http request.
        /// </summary>
        public Dictionary<string, string> Query { get; set; }

        /// <summary>
        /// Gets the Cookies of the Http request.
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// Gets the Body of the Http request.
        /// </summary>
        public object? Body { get; set; }

        /// <summary>
        /// Gets the Url of the Http request.
        /// </summary>
        public string Path { get; set; } = "";

        /// <summary>
        /// Gets the Protocol of the Http request.
        /// </summary>
        public string Protocol { get; set; } = "";

        /// <summary>
        /// Gets the Method of the Http request.
        /// </summary>
        public string Method { get; set; } = "GET";

        /// <summary>
        /// Constructor for <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest()
        {
            Query = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Cookies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
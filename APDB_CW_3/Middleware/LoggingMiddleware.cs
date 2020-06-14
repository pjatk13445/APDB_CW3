using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace APDB_CW_3.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string requestBody = "";
                using (var reader = new StreamReader(
                    httpContext.Request.Body,
                    Encoding.UTF8,
                    true,
                    512,
                    true
                ))
                {
                    requestBody = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                }

                using (StreamWriter writer = new StreamWriter(@"logs/request.log", true))
                {
                    var entry = new Dictionary<string, string>
                    {
                        {"Method", httpContext.Request.Method},
                        {"Path", httpContext.Request.Path},
                        {"QueryString", httpContext.Request?.QueryString.ToString()},
                        {"Body", requestBody},
                    };
                    writer.WriteLine(JsonSerializer.Serialize(entry));
                }
            }

            await _next(httpContext);
        }
    }
}
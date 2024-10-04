using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureHealth.DataServices.Pipelines;

namespace HealthToolkit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FhirController : ControllerBase
    {
        private readonly IPipeline<HttpRequestMessage, HttpResponseMessage> pipeline;

        public FhirController(IPipeline<HttpRequestMessage, HttpResponseMessage> pipeline)
        {
            this.pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        [HttpGet(Name = "GetPatients")]
        public async Task<IActionResult> Get()
        {
            await this.pipeline.ExecuteAsync(CreateHttpRequestMessage(this.Request)).ConfigureAwait(false);

            return Ok();
        }

        private static HttpRequestMessage CreateHttpRequestMessage(HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.PathBase.Add(request.Path),
                Query = request.QueryString.ToString()
            };

            if (request.Host.Port.HasValue)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }

            var requestMethod = request.Method;
            var requestMessage = new HttpRequestMessage(new HttpMethod(requestMethod), uriBuilder.Uri);

            if (request.Body != null)
            {
                requestMessage.Content = new StreamContent(request.Body);
            }

            requestMessage.Headers.Host = request.Host.Host;
            foreach (var header in request.Headers)
            {
                var headers = header.Value.ToArray();
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, headers) && requestMessage.Content != null)
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, headers);
                }
            }

            return requestMessage;
        }
    }
}

using Microsoft.AzureHealth.DataServices.Pipelines;

namespace HealthToolkit
{
    public class SimpleInputFilterOptions
    {
        public HttpMethod HttpMethod { get; set; }

        public Uri BaseUrl { get; set; }

        public string Path { get; set; }

        public StatusType ExecutionStatus { get; set; }
    }
}

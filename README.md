Sample repro to aid in debugging for issue: https://github.com/microsoft/azure-health-data-services-toolkit/issues/140

To reproduce the stream issue, open the `HealthToolkit.sln` and run it using the `docker-compose` profile.

![Run profile](https://github.com/radupurdea/azure-healthdata-toolkit-stream-issue/blob/main/Images/Step1.JPG)

Next, run the HealthToolkit api and copy the URL of the API.

![Copy url](https://github.com/radupurdea/azure-healthdata-toolkit-stream-issue/blob/main/Images/Step2.JPG)

Then, open the `ClientApp.sln` and paste the URL in the `Program.cs` at the `fhirServelUrl` variable.

The `ClientApp` will attempt to send requests to the FHIR API in a loop as it seems this i the easiest way to have the error occur. 

Note: The FHIR request itself will end up in a 404, but this is intentional as the actual issue occurs before the binding is executing the actual http call.

Run the `ConsoleApp` and check the API project logs. You should see some requests fail with the below error.

![Run client](https://github.com/radupurdea/azure-healthdata-toolkit-stream-issue/blob/main/Images/Step3.JPG)

![See error](https://github.com/radupurdea/azure-healthdata-toolkit-stream-issue/blob/main/Images/Step4.JPG)

Stack trace:

HealthToolkit | info: Microsoft.AzureHealth.DataServices.Pipelines.WebPipeline[0]

HealthToolkit |       Pipeline WebPipeline-93d903df-0706-4ab2-ae17-0458bdb10eae filter SimpleInputFilter-7ce39c06-56fb-4bcd-877b-1030aacac942 executed with status Any.

HealthToolkit | info: HealthToolkit.SimpleInputFilter[0]

HealthToolkit |       Entered SimpleInputFilter

HealthToolkit | info: Microsoft.AzureHealth.DataServices.Bindings.RestBinding[0]

HealthToolkit |       RestBinding-144fe72a-9e91-4a28-bf84-54ddb5b4bed9 binding received.

HealthToolkit | fail: Microsoft.AzureHealth.DataServices.Bindings.RestBinding[0]

HealthToolkit |       RestBinding-144fe72a-9e91-4a28-bf84-54ddb5b4bed9 fault with server request.

HealthToolkit |       System.InvalidOperationException: The stream was already consumed. It cannot be read again.

HealthToolkit |          at System.Net.Http.StreamContent.PrepareContent()

HealthToolkit |          at System.Net.Http.HttpContent.LoadIntoBufferAsync(Int64 maxBufferSize, CancellationToken cancellationToken)

HealthToolkit |          at System.Net.Http.HttpContent.ReadAsByteArrayAsync(CancellationToken cancellationToken)

HealthToolkit |          at Microsoft.AzureHealth.DataServices.Bindings.RestBinding.ExecuteAsync(OperationContext context)

HealthToolkit | fail: Microsoft.AzureHealth.DataServices.Pipelines.WebPipeline[0]

HealthToolkit |       Pipeline WebPipeline-93d903df-0706-4ab2-ae17-0458bdb10eae binding RestBinding- 144fe72a-9e91-4a28-bf84-54ddb5b4bed9 error.

HealthToolkit |       System.InvalidOperationException: The stream was already consumed. It cannot be read again.

HealthToolkit |          at System.Net.Http.StreamContent.PrepareContent()

HealthToolkit |          at System.Net.Http.HttpContent.LoadIntoBufferAsync(Int64 maxBufferSize, CancellationToken cancellationToken)

HealthToolkit |          at System.Net.Http.HttpContent.ReadAsByteArrayAsync(CancellationToken cancellationToken)

HealthToolkit |          at Microsoft.AzureHealth.DataServices.Bindings.RestBinding.ExecuteAsync(OperationContext context)

HealthToolkit | info: Microsoft.AzureHealth.DataServices.Bindings.RestBinding[0]

HealthToolkit |       RestBinding-144fe72a-9e91-4a28-bf84-54ddb5b4bed9 signaled error.

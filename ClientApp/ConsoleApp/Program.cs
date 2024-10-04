Console.WriteLine("Hello, World!");

var fhirServelUrl = new Uri("https://localhost:25761/Fhir");
var client = new HttpClient();
var content = await File.ReadAllBytesAsync("bundle.json").ConfigureAwait(false);

for (var i = 0; i < 100; i++)
{
    var payload = new ByteArrayContent(content);
    var request = new HttpRequestMessage(HttpMethod.Get, fhirServelUrl);

    request.Content = payload;
    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

    var response = await client.SendAsync(request);

    try
    {
        response.EnsureSuccessStatusCode();

        Console.WriteLine($"FHIR resources created for resource {i}");
    }
    catch (Exception ex)
    {
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"ERROR for resource {i}");
        Console.WriteLine(result);
    }
}
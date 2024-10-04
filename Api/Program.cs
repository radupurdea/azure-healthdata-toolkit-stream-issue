using HealthToolkit;
using Microsoft.AzureHealth.DataServices.Bindings;
using Microsoft.AzureHealth.DataServices.Configuration;
using Microsoft.AzureHealth.DataServices.Pipelines;

var builder = WebApplication.CreateBuilder(args);

builder.Services.UseWebPipeline();

builder.Services.AddInputFilter<SimpleInputFilterOptions>(typeof(SimpleInputFilter), options =>
{
    options.BaseUrl = new Uri("http://hapi.fhir.org/baseR4");
    options.HttpMethod = HttpMethod.Post;
    options.Path = "Bundle";
    options.ExecutionStatus = StatusType.Any;
});

builder.Services.AddBinding<RestBinding, RestBindingOptions>(o =>
{
    o.BaseAddress = new Uri("http://hapi.fhir.org/baseR4");
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

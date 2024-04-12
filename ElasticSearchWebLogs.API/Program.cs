using ElasticSearchWebLogs.API.ElasticSearch;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
               .BasicAuthentication("elastic", "elastic")
               .DefaultIndex("kibana_sample_data_logs")
               .DisableDirectStreaming();

var client = new ElasticClient(settings);
builder.Services.AddSingleton(client);

builder.Services.AddScoped<ElasticSearchServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

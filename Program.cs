using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using GloboClimaAPI.Services;
using System;
using Microsoft.AspNetCore.Builder;
using Amazon.DynamoDBv2.DataModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<GloboClimaService>();
builder.Services.AddScoped<DynamoDBService>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();

var credentials = new BasicAWSCredentials("AKIA6K5V7P56SBS2AY4T", "L3Zd6N3jron7Rh3vfT1/emQ0rQSNJmOWPCd5a8r/");
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddAWSService<IAmazonDynamoDB>(new AWSOptions
{
    Credentials = credentials,
    Region = Amazon.RegionEndpoint.USEast2 // Altere para sua região
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

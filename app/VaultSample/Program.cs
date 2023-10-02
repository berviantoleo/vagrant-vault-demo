using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Vault;
using Vault.Client;
using Vault.Model;
using VaultSample;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddSingleton((serviceProvider) =>
{
    string address = builder.Configuration["Vault:Address"];
    VaultConfiguration config = new(address);

    VaultClient vaultClient = new(config);
    vaultClient.SetToken(builder.Configuration["Vault:Token"]);
    return vaultClient;
});

// Add services to the container.

builder.Services.AddControllers();
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

app.MapGet("/", () => {
    return "OK";
});

var secretsApi = app.MapGroup("/secrets");
var mountPath = "secret";

secretsApi.MapPost("/", ([FromServices]ILoggerFactory loggerFactory, [FromServices] VaultClient vaultClient, [FromBody] VaultData secretData) =>
{
    var logger = loggerFactory.CreateLogger("vault");
    // Write a secret
    try {
        var kvRequestData = new KvV2WriteRequest(secretData.Data);
        vaultClient.Secrets.KvV2Write(secretData.Name, kvRequestData, mountPath);
        return Results.Ok("OK");
    } catch (Exception ex)
    {
        logger.LogError(ex, "general error");
        return Results.NoContent();
    }
});

secretsApi.MapGet("/{secretKey}", ([FromServices]ILoggerFactory loggerFactory, [FromServices] VaultClient vaultClient, string secretKey) =>
{
    var logger = loggerFactory.CreateLogger("vault");
    try
    {
        VaultResponse<KvV2ReadResponse> resp = vaultClient.Secrets.KvV2Read(secretKey, mountPath);
        return resp.Data != null ? Results.Ok(resp.Data) : Results.NotFound();
    }
    catch (VaultApiException ex)
    {
        logger.LogError(ex, "vault error");
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "general error");
        return Results.NotFound();
    }
});


app.Run();


using CreditScoringEngine.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;

namespace CreditScoringEngine.Tests.Integration;

public class ClienteEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ClienteEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetClients_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Cliente");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task PostClients_ReturnsOk()
    {
        var user = new Cliente
        {
            Nome = "Cliente Test",
            Idade = 40,
            RendaMensal = 10000,
            HistoricoCreditoSimulado = ""
        };
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");


        var response = await _client.PostAsync("/api/Cliente", content);
        
        
        response.EnsureSuccessStatusCode();
    }
}

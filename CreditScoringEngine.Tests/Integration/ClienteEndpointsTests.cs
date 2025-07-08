
using CreditScoringEngine.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
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

    [Fact]
    public async Task GetClientById_ReturnsNotFound_WhenNotExists()
    {
        var idInexistente = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/Cliente/{idInexistente}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostClient_ReturnsBadRequest_WhenBodyInvalid()
    {
        var json = "{}"; // Corpo incompleto

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/Cliente", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutClient_ReturnsOk_WhenSuccessful()
    {
        // Primeiro cria o cliente
        var cliente = new Cliente
        {
            Nome = "Fulano",
            Idade = 30,
            RendaMensal = 5000,
            HistoricoCreditoSimulado = "Bom pagador"
        };

        var postContent = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
        var postResponse = await _client.PostAsync("/api/Cliente", postContent);
        var criado = JsonSerializer.Deserialize<Cliente>(
            await postResponse.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Agora atualiza
        var novoDto = new
        {
            Nome = "Fulano Atualizado",
            Idade = 35,
            RendaMensal = 6000,
            HistoricoCreditoSimulado = "Excelente"
        };

        var putContent = new StringContent(JsonSerializer.Serialize(novoDto), Encoding.UTF8, "application/json");
        var putResponse = await _client.PutAsync($"/api/Cliente/{criado.Id}", putContent);

        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteClient_ReturnsNoContent_WhenSuccessful()
    {
        // Cria o cliente
        var cliente = new Cliente
        {
            Nome = "Para Deletar",
            Idade = 50,
            RendaMensal = 2000,
            HistoricoCreditoSimulado = "Negativado"
        };

        var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
        var postResponse = await _client.PostAsync("/api/Cliente", content);
        var criado = JsonSerializer.Deserialize<Cliente>(
            await postResponse.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Deleta
        var deleteResponse = await _client.DeleteAsync($"/api/Cliente/{criado.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteClient_ReturnsNotFound_WhenNotExists()
    {
        var response = await _client.DeleteAsync($"/api/Cliente/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}

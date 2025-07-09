using CreditScoringEngine.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using System.Net;
using CreditScoringEngine.Domain.DTOs;
using System.Text.Json.Serialization;

namespace CreditScoringEngine.Tests.Integration;

public class PropostaEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PropostaEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<Guid> CriarClienteAsync()
    {
        var cliente = new Cliente
        {
            Nome = "Proposta Client",
            Idade = 40,
            RendaMensal = 9000,
            HistoricoCreditoSimulado = "Bom"
        };
        var json = JsonSerializer.Serialize(cliente);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/Cliente", content);
        response.EnsureSuccessStatusCode();

        var criado = JsonSerializer.Deserialize<Cliente>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return criado.Id;
    }
    [Fact]
    public async Task PostProposta_ReturnsCreated()
    {
        var clienteId = await CriarClienteAsync();

        var proposta = new { ValorSolicitado = 10000, ClienteId = clienteId };
        var json = JsonSerializer.Serialize(proposta);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/Proposta", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetAllPropostas_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Proposta");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPropostaById_ReturnsNotFound_WhenInexistente()
    {
        var response = await _client.GetAsync($"/api/Proposta/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProposta_RetornaNoContent()
    {
        var clienteId = await CriarClienteAsync();

        var proposta = new PropostaDto(15000, clienteId);
        var json = JsonSerializer.Serialize(proposta, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var post = await _client.PostAsync("/api/Proposta/", content);

        var criada = JsonSerializer.Deserialize<PropostaResponseDto>(
            await post.Content.ReadAsStringAsync(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });


        var delete = await _client.DeleteAsync($"/api/Proposta/{criada.PropostaId}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

}


using Microsoft.AspNetCore.Mvc.Testing;
using MinimalAPIKickoff.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace MinimalAPIKickoff.Tests.Integration;

public class UserEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/User");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task PostUser_ReturnsOk()
    {
        var user = new User
        {
            Name = "Post_endpoint",
            Email = "post@mail.com"
        };
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");


        var response = await _client.PostAsync("/api/User", content);
        
        
        response.EnsureSuccessStatusCode();
    }
}

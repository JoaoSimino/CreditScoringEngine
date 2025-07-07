using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Serilog;

namespace CreditScoringEngine.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes) 
    {
        var group = routes.MapGroup("/api/Cliente").WithTags(nameof(Cliente));

        group.MapGet("/", async (IClienteService service) =>
        {
            var listaDeUsuarios = await service.GetAllAsync();
            
            return listaDeUsuarios;
        })
        .WithName("GetAllClientes")
        .WithOpenApi();

        group.MapGet("/{id}", async (Guid id, IClienteService service) =>
        {
            var cliente = await service.GetByIdAsync(id);

            Log.Information("Consulta ao cliente {cliente} foi efetuada no sistema!", cliente.Nome);
            return cliente;
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapPost("/", async (Cliente cliente, IClienteService service) =>
        {
            
            await service.AddAsync(cliente);

            Log.Information("Cliente {id}:{user} cadastrado com sucesso!", cliente.Id, cliente.Nome);
            return TypedResults.Created($"/api/User/{cliente.Id}", cliente);
        })
        .WithName("CreateUser")
        .WithOpenApi();

    }
}

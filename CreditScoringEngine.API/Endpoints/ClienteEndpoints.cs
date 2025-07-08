using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Domain.DTOs;
using CreditScoringEngine.Domain.Entities;
using Serilog;

namespace CreditScoringEngine.API.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints(this IEndpointRouteBuilder routes) 
    {
        var group = routes.MapGroup("/api/Cliente").WithTags(nameof(Cliente));

        group.MapGet("/", async (IClienteService service) =>
        {
            var listaDeClientes = await service.GetAllAsync();
            
            return listaDeClientes;
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

        group.MapPost("/", async (ClienteDto clienteDto, IClienteService service) =>
        {
            var cliente = new Cliente().DtoToEntity(clienteDto);
            await service.AddAsync(cliente);

            Log.Information("Cliente {id}:{user} cadastrado com sucesso!", cliente.Id, cliente.Nome);
            return TypedResults.Created($"/api/User/{cliente.Id}", cliente);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapPut("/{id}",async (IClienteService service, Guid id, ClienteDto clienteDto) => {
            
            var cliente = await service.GetByIdAsync(id);
            cliente!.Update(clienteDto);
            await service.UpdateAsync(cliente!);

            return TypedResults.Ok<Cliente>(cliente!);
        });
        group.MapDelete("/{id}", async  (IClienteService service, Guid id) => {
            var cliente = await service.GetByIdAsync(id);
            await service.DeleteAsync(cliente!);
            return TypedResults.NoContent();
        });

    }
}

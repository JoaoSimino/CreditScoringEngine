﻿using CreditScoringEngine.Application.Services;
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
            var clientes = await service.GetAllAsync();
            

            return clientes.Select(c=> new
            {
                c.Id,
                c.Nome,
                c.RendaMensal,
                c.Idade,
                c.HistoricoCreditoSimulado
            });
        })
        .WithName("GetAllClients")
        .WithOpenApi();

        group.MapGet("/{id}", async (Guid id, IClienteService service) =>
        {
            var cliente = await service.GetByIdAsync(id);

            Log.Information("Consulta ao cliente {cliente} foi efetuada no sistema!", cliente.Nome);
            var resultado = new
            {
                cliente.Id,
                cliente.Nome,
                cliente.RendaMensal,
                cliente.Idade,
                cliente.HistoricoCreditoSimulado
            };
            
            return resultado;
        })
        .WithName("GetClientById")
        .WithOpenApi();

        group.MapPost("/", async (ClienteDto clienteDto, IClienteService service) =>
        {
            var cliente = new Cliente().DtoToEntity(clienteDto);
            await service.AddAsync(cliente);

            Log.Information("Cliente {id}:{user} cadastrado com sucesso!", cliente.Id, cliente.Nome);
            return TypedResults.Created($"/api/User/{cliente.Id}", cliente);
        })
        .WithName("CreateClient")
        .WithOpenApi();

        group.MapPut("/{id}",async (IClienteService service, Guid id, ClienteDto clienteDto) => {
            
            var cliente = await service.GetByIdAsync(id);
            cliente!.Update(clienteDto);
            await service.UpdateAsync(cliente!);

            return TypedResults.Ok<Cliente>(cliente!);
        }).
        WithName("UpdateClient")
        .WithOpenApi();

        group.MapDelete("/{id}", async  (IClienteService service, Guid id) => {
            var cliente = await service.GetByIdAsync(id);
            await service.DeleteAsync(cliente!);
            return TypedResults.NoContent();
        })
        .WithName("DeleteClient")
        .WithOpenApi();

    }
}

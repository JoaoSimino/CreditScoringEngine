using Microsoft.EntityFrameworkCore;
using MinimalAPIKickoff.Application.Services;
using MinimalAPIKickoff.Domain.DTOs;
using MinimalAPIKickoff.Domain.Entities;
using MinimalAPIKickoff.Infrastructure.Data;
using Serilog;

namespace MinimalAPIKickoff.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes) 
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/", async (IUserService service) =>
        {
            var listaDeUsuarios = await service.GetAllAsync();
            
            return listaDeUsuarios;
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/{id}", async (Guid id, IUserService service) =>
        {
            var user = await service.GetByIdAsync(id);

            Log.Information("Consulta ao usuario {user} foi efetuada no sistema!", user.Name);
            return user;
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapPost("/", async (UserDto userDto, IUserService service) =>
        {
            var user = new User().DtoToEntity(userDto);
            await service.AddAsync(user);

            Log.Information("Usuario {id}:{user} cadastrado com sucesso!", user.Id, user.Name);
            return TypedResults.Created($"/api/User/{user.Id}", user);
        })
        .WithName("CreateUser")
        .WithOpenApi();

    }
}

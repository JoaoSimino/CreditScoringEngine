using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Domain.DTOs;
using CreditScoringEngine.Domain.Entities;
using Serilog;

namespace CreditScoringEngine.API.Endpoints;

public static class PropostaEndpoints
{
    public static void MapPropostaEndpoints(this IEndpointRouteBuilder routes) 
    {
        var group = routes.MapGroup("/api/Proposta").WithTags(nameof(PropostaCredito));
        
        group.MapGet("/", async (IPropostaService service) =>
        {
            var listaDePropostas = await service.GetAllAsync();

            return listaDePropostas.Select(p => new{
                p.Id,
                p.ValorSolicitado,
                p.DataProposta,
                p.Status,
                p.Score,
                p.Justificativa,
                p.ClienteId
            });
        })
        .WithName("GetAllProposals")
        .WithOpenApi();

        group.MapGet("/{id}", async (Guid id, IPropostaService service) =>
        {
            var proposta = await service.GetByIdAsync(id);

            Log.Information("Consulta a Proposta {proposta} foi efetuada no sistema!", proposta.Id);

            var resultado = new
            {
                proposta.Id,
                proposta.ValorSolicitado,
                proposta.DataProposta,
                proposta.Status,
                proposta.Score,
                proposta.Justificativa,
                proposta.ClienteId
            };
            
            return resultado;
        })
        .WithName("GetProposalById")
        .WithOpenApi();

        group.MapPost("/", async (PropostaDto propostaDto, IPropostaService service, IClienteService clientService) =>
        {
            var cliente = await clientService.GetByIdAsync(propostaDto.ClienteId);//verificando inicialmente se o cliente eh valido, se nao a funcao ja lanca excessao

            //verificar se o ID do cliente ja esta associado a alguma proposta, bucasndo na tabela
            await service.GetPropostaByClientIdAsync(cliente!.Id);

            var proposta = new PropostaCredito().DtoToEntity(propostaDto, cliente!);

            await service.AddAsync(proposta);

            Log.Information("Proposta {id}:{clienteId} cadastrada com sucesso!", cliente!.Id, proposta.Id);
            return TypedResults.Created($"/api/Proposta/{proposta.Id}", new
            {
                PropostaId = proposta.Id,
                ValorSolicitado = proposta.ValorSolicitado,
                DataDeCriacao = proposta.DataProposta,
                Status = proposta.Status,
                ClienteId = proposta.ClienteId
            });
        })
        .WithName("CreateProposal")
        .WithOpenApi();

        group.MapPut("/{id}", async (IPropostaService service, IClienteService clientService, Guid proposalId, PropostaDto propostaDto) => {

            var proposta = await service.GetByIdAsync(proposalId);//ver inicialmente se a proposta existe na base
            var cliente = await clientService.GetByIdAsync(propostaDto.ClienteId);//se a proposta existir e eu nao mudar o cliente ja foi validado se ele existe! sendo possivel trocar o cliente preciso verificar se o novo esta na base
            
            proposta!.Update(propostaDto,cliente);
            await service.UpdateAsync(proposta!);

            return TypedResults.Ok<PropostaCredito>(proposta!);
        }).
        WithName("UpdateProposal")
        .WithOpenApi();

        group.MapDelete("/{id}", async (IPropostaService service, Guid id) => {
            
            var proposta = await service.GetByIdAsync(id);
            await service.DeleteAsync(proposta!);
            return TypedResults.NoContent();
        })
        .WithName("DeleteProposal")
        .WithOpenApi();


    }
}

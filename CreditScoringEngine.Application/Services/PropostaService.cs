using CreditScoringEngine.Application.Exceptions;
using CreditScoringEngine.Domain.Entities;
using CreditScoringEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditScoringEngine.Application.Services;

public class PropostaService : CrudService<PropostaCredito>, IPropostaService
{
    public PropostaService(CreditScoringEngineContext context) : base(context)
    {
    }

    public override async Task AddAsync(PropostaCredito proposta)
    {
        try
        {
            await base.AddAsync(proposta);
        }
        catch (DbUpdateException ex)
        {

            throw new PropostaExceptions($"Erro ao salvar cliente: verifique se todos os campos obrigatórios foram preenchidos.{ex.Message}");
        }
        catch (Exception ex)
        {
            throw new PropostaExceptions($"Erro inesperado ao adicionar cliente.{ex.Message}");
        }

    }

    public override async Task<PropostaCredito?> GetByIdAsync(object id)
    {
        var proposta = await _context.Set<PropostaCredito>().FindAsync(id);

        if (proposta is null)
        {
            throw new PropostaNotFoundExceptions("Proposta invalida, nao encontrado na base!");
        }
        return proposta;
    }


    public override async Task UpdateAsync(PropostaCredito proposta)
    {
        try
        {
            await base.UpdateAsync(proposta);
        }
        catch (Exception ex)
        {

            throw new PropostaExceptions($"Problema ao atualizar a Proposta!{ex.Message}");
        }
    }

    public override async Task DeleteAsync(PropostaCredito proposta)
    {
        try
        {
            _context.Set<PropostaCredito>().Remove(proposta);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new PropostaExceptions($"Problema para remover a Proposta da base!.{ex.Message}");
        }
    }

    public async Task GetPropostaByClientIdAsync(Guid clientId)
    {
        var proposta = await _context.Set<PropostaCredito>()
            .Where(p => p.ClienteId == clientId)
            .FirstOrDefaultAsync();

        if (proposta is not null) 
        {
            throw new PropostaExceptions($"O Cliente em questao ja esta sendo utilizado em outra Proposta!");
        }


    }

    public async Task<IEnumerable<PropostaCredito>> GetPropostaByStatusAsync(StatusProposta status)
    {
        IEnumerable<PropostaCredito>? propostas = await _context.Set<PropostaCredito>()
            .Include(pc => pc.Cliente)//eager loading
            .Where(pc => pc.Status == status)
            .ToListAsync();
        return propostas;   
    }
}

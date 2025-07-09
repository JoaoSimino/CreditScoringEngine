using CreditScoringEngine.Application.Exceptions;
using CreditScoringEngine.Domain.Entities;
using CreditScoringEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditScoringEngine.Application.Services;

public class ClienteService : CrudService<Cliente>, IClienteService
{

    public ClienteService(CreditScoringEngineContext context) : base(context)
    {

    }
    //specific queries must be created in the interface associated with this class and implemented here!

    //overriding the inherited default behavior, to put a trade check


    public override async Task AddAsync(Cliente cliente)
    {
        try
        {
            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {

            throw new ClienteExceptions($"Erro ao salvar cliente: verifique se todos os campos obrigatórios foram preenchidos.{ex.Message}");
        }
        catch (Exception ex)
        {
            throw new ClienteExceptions($"Erro inesperado ao adicionar cliente.{ex.Message}");
        }

    }

    public override async Task<Cliente?> GetByIdAsync(object id)
    {
        var cliente = await _context.Set<Cliente>()
            .FindAsync(id);

        if (cliente is null)
        {
            throw new ClienteNotFoundException("Cliente invalido, nao encontrado na base!");
        }


        return cliente;
    }


    public override async Task UpdateAsync(Cliente cliente)
    {
        try
        {
            await base.UpdateAsync(cliente);
        }
        catch (Exception ex)
        {

            throw new ClienteExceptions($"Problema ao atualizar Cliente!{ex.Message}");
        }
    }

    public override async Task DeleteAsync(Cliente cliente)
    {
        try
        {
            _context.Set<Cliente>().Remove(cliente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new ClienteExceptions($"Problema para remover Cliente da base!.{ex.Message}");
        }
    }

    

}

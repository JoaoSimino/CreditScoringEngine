using CreditScoringEngine.Application.Exceptions;
using CreditScoringEngine.Domain.Entities;
using CreditScoringEngine.Infrastructure.Data;

namespace CreditScoringEngine.Application.Services;

public class ClienteService : CrudService<Cliente>, IClienteService
{

    public ClienteService(CreditScoringEngineContext context) : base(context)
    {

    }
    //specific queries must be created in the interface associated with this class and implemented here!

    //overriding the inherited default behavior, to put a trade check
    public override async Task<Cliente?> GetByIdAsync(object id)
    {
        var user = await _context.Set<Cliente>().FindAsync(id);
        
        if (user is null) 
        {
            throw new ClienteExceptions("Cliente invalido, nao encontrado na base!");
        }
        return user;
    }

}

using Microsoft.EntityFrameworkCore;
using MinimalAPIKickoff.Application.Exceptions;
using MinimalAPIKickoff.Domain.Entities;
using MinimalAPIKickoff.Infrastructure.Data;

namespace MinimalAPIKickoff.Application.Services;

public class UserService : CrudService<User>, IUserService
{

    public UserService(MinimalAPIKickoffContext context) : base(context)
    {

    }
    //specific queries must be created in the interface associated with this class and implemented here!

    //overriding the inherited default behavior, to put a trade check
    public override async Task<User?> GetByIdAsync(object id)
    {
        var user = await _context.Set<User>().FindAsync(id);
        
        if (user is null) 
        {
            throw new UserExceptions("Usuario invalido, nao encontrado na base!");
        }
        return user;
    }

}

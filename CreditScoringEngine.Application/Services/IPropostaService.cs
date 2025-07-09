using CreditScoringEngine.Domain.Entities;

namespace CreditScoringEngine.Application.Services;

public interface IPropostaService : ICrudService<PropostaCredito>
{
    public Task GetPropostaByClientIdAsync(Guid clientId);
    public Task<IEnumerable<PropostaCredito>> GetPropostaByStatusAsync(StatusProposta status);
}

using CreditScoringEngine.Domain.Entities;

namespace CreditScoringEngine.Application.Services;

public class PropostaProcessingService : IPropostaProcessingService
{
    private readonly IPropostaService _propostaService;

    public PropostaProcessingService(IPropostaService propostaService)
    {
        _propostaService = propostaService;
    }
    public async Task ProcessarPropostasPendentesAsync()
    {
        var propostas = await _propostaService.GetPropostaByStatusAsync(StatusProposta.Pendente);

        if (propostas.Count() == 0) 
        {
            Console.WriteLine("Sem propostas para tratar no momento!");
        }

        foreach (var proposta in propostas)
        {
            //aqui entra a logica de negocio que sera desenvolvida para tratamento de cada proposta!
            proposta.Justificativa = "Aprovado automaticamente";
            proposta.Status = StatusProposta.Aprovada;

            await _propostaService.UpdateAsync(proposta);
        }

    }
}

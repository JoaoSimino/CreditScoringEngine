
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
            var cliente = proposta.Cliente;

            int score = 0;

            if (cliente.RendaMensal >= 10000) score += 40;
            else if (cliente.RendaMensal >= 5000) score += 25;
            else score += 10;


            if (cliente.Idade >= 30 && cliente.Idade <= 50) score += 30;
            else if (cliente.Idade >= 21) score += 20;
            else score += 10;

            if (cliente.HistoricoCreditoSimulado.Contains("sem inadimplência", StringComparison.OrdinalIgnoreCase))
                score += 30;
            else if (cliente.HistoricoCreditoSimulado.Contains("inadimplente", StringComparison.OrdinalIgnoreCase))
                score -= 20;

            FaixaRisco faixa = score switch
            {
                >= 80 => FaixaRisco.Baixo,
                >= 50 => FaixaRisco.Medio,
                _ => FaixaRisco.Alto
            };

            StatusProposta status;
            string justificativa;

            switch (faixa)
            {
                case FaixaRisco.Baixo:
                    status = StatusProposta.Aprovada;
                    justificativa = "Score elevado e baixo risco.";
                    break;

                case FaixaRisco.Medio:
                    status = StatusProposta.Aprovada;
                    justificativa = "Risco moderado. Proposta aprovada com ressalvas.";
                    break;

                default:
                    status = StatusProposta.Recusada;
                    justificativa = "Alto risco de inadimplência.";
                    break;
            }

            proposta.Status = status;
            proposta.Justificativa = justificativa;

            proposta.Score = new ScoreInterno
            {
                Valor = score,
                Faixa = faixa
            };


            await _propostaService.UpdateAsync(proposta);
        }

    }
}

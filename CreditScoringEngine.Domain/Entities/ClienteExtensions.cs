using CreditScoringEngine.Domain.DTOs;

namespace CreditScoringEngine.Domain.Entities;

public static class ClienteExtensions
{
    public static Cliente DtoToEntity(this Cliente cliente,ClienteDto clienteDto) 
    {
        cliente.Id = Guid.NewGuid();
        cliente.Nome = clienteDto.Nome;
        cliente.RendaMensal = clienteDto.RendaMensal;
        cliente.Idade = clienteDto.Idade;
        cliente.HistoricoCreditoSimulado = clienteDto.HistoricoCreditoSimulado;

        return cliente;
    }

    public static Cliente Update(this Cliente cliente, ClienteDto clienteDto) 
    {
        cliente.Nome = clienteDto.Nome;
        cliente.RendaMensal = clienteDto.RendaMensal;
        cliente.Idade = clienteDto.Idade;
        cliente.HistoricoCreditoSimulado = clienteDto.HistoricoCreditoSimulado;

        return cliente;
    }
}

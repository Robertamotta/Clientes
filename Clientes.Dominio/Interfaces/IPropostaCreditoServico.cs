using Clientes.Dominio.DTOs;

namespace Clientes.Dominio.Interfaces;

public interface IPropostaCreditoServico
{
    Task SalvarPropostaCredito(PropostaCreditoDto propostaCredito);
}

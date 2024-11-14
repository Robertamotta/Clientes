using Clientes.Dominio.DTOs;

namespace Clientes.Dominio.Interfaces;

public interface IEmissaoCartaoCreditoServico
{
    Task SalvarEmissaoCartaoCredito(EmissaoCartaoCreditoDto emissaoCartaoCredito);
}

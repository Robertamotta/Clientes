using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using Clientes.Infraestrutura.Interfaces;

namespace Clientes.Aplicacao.Servicos;

public class EmissaoCartaoCreditoServico(IClienteRepositorio clienteRepositorio) : IEmissaoCartaoCreditoServico
{
    public async Task SalvarEmissaoCartaoCredito(EmissaoCartaoCreditoDto emissaoCartaoCredito)
    {
        var atualizacaoEmissaoCartaoCredito = emissaoCartaoCredito.ParaAtualizacaoQtdCartoesEmitidos();

        await clienteRepositorio.Atualizar(atualizacaoEmissaoCartaoCredito);
    }
}

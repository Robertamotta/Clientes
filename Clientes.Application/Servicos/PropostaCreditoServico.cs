using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using Clientes.Infraestrutura.Interfaces;

namespace Clientes.Aplicacao.Servicos;

public class PropostaCreditoServico(IClienteRepositorio clienteRepositorio) : IPropostaCreditoServico
{
    public async Task SalvarPropostaCredito(PropostaCreditoDto propostaCredito)
    {
        var atualizacaoPropostaCredito = propostaCredito.ParaAtualizacaoPropostaCredito();

        await clienteRepositorio.Atualizar(atualizacaoPropostaCredito);
    }
}

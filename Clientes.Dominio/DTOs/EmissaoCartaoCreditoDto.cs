using Clientes.Dominio.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace Clientes.Dominio.DTOs;

[ExcludeFromCodeCoverage]
public class EmissaoCartaoCreditoDto
{
    public int Id { get; set; }
    public int QtdCartoesEmitidos {  get; set; }

    public virtual Cliente ParaAtualizacaoQtdCartoesEmitidos()
    {
        return new Cliente
        {
            Id = Id,
            QtdCartoesEmitidos = QtdCartoesEmitidos
        };
    }
}

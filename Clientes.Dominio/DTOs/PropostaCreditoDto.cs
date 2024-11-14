using Clientes.Dominio.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace Clientes.Dominio.DTOs;

[ExcludeFromCodeCoverage]
public class PropostaCreditoDto
{
    public int Id { get; set; }
    public bool AprovacaoCredito {  get; set; }

    public virtual Cliente ParaAtualizacaoPropostaCredito()
    {
        return new Cliente
        {
            Id = Id,
            AprovacaoCredito = AprovacaoCredito
        };
    }
}

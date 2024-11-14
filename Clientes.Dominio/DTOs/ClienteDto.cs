using System.Diagnostics.CodeAnalysis;

namespace Clientes.Dominio.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ClienteDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public decimal Renda { get; set; }
        public int ScoreCredito { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int QtdCartoesEmitidos { get; set; }
    }
}

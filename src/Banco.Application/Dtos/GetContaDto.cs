namespace Banco.API.Application.Dtos
{
    public class GetContaDto
    {
        public int Id { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public string Agencia { get; set; } = string.Empty;
        public double Saldo { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public int ClienteId { get; set; }

        public List<LinkDto> Links { get; set; } = new();
    }
}

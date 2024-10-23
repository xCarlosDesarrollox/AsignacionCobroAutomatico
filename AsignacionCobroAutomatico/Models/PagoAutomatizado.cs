namespace AsignacionCobroAutomatico.Models
{
    public class PagoAutomatizado
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int TarjetaId { get; set; }
        public int ServicioId { get; set; }
        public decimal Monto { get; set; }
        public string NumeroReferencia { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Today;
        public int EstatusId { get; set; }
    }
}

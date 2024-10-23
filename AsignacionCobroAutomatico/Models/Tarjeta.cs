using System.ComponentModel;

namespace AsignacionCobroAutomatico.Models
{
    public class Tarjeta
    {
        public int Id { get; set; }
        public string NumeroTarjeta { get; set; }
        public string PinSeguridad { get; set; }
        public string FechaExpiracion { get; set; }
        [DisplayName("Tipo tarjeta")]
        public int TipoTarjetaId { get; set;}
        public int EmpresaEmisoraId { get; set; }
        public int ClienteId { get; set; }

    }
}

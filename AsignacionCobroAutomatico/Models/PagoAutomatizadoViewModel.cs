using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AsignacionCobroAutomatico.Models
{
    public class PagoAutomatizadoViewModel
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public string NombreServicio { get; set; }
        public string NumeroReferencia { get; set; }
        public string NombreTipoNotificación { get; set; }
        public decimal Monto { get; set; }
    }
}

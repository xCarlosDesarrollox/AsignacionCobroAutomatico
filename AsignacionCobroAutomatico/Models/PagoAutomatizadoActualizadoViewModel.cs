using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsignacionCobroAutomatico.Models
{
    public class PagoAutomatizadoActualizadoViewModel
    {
        public Cliente cliente { get; set; }
        public Tarjeta tarjeta { get; set; }
        public string Empresa { get; set; }
        public IEnumerable<ServicioCliente> servicio { get; set; }
        public IEnumerable<SelectListItem> TipoTarjeta { get; set; }

    }
}

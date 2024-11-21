using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsignacionCobroAutomatico.Models
{
    public class TarjetaViewModel: Tarjeta
    {
        public string Empresa { get; set; }
        public IEnumerable<SelectListItem> TipoTarjeta { get; set; }
    }
}

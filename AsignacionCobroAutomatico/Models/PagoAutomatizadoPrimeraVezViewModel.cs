using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AsignacionCobroAutomatico.Models
{
    public class PagoAutomatizadoPrimeraVezViewModel
    {
        public Tarjeta tarjeta { get; set; }
        public Cliente cliente { get; set; }
        public string EmpresaNombre { get; set; }
        public IEnumerable<SelectListItem> TipoTarjeta { get; set; }
        public IEnumerable<SelectListItem> ServicioFijo { get; set; }
        [Display(Name = "AgregarServicio")]
        public List<int> ServiciosSeleccionados { get; set; }
    }
}

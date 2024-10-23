using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsignacionCobroAutomatico.Models
{
    public class ClienteViewModel: Cliente
    {
        public IEnumerable<SelectListItem> notificacion {  get; set; } 
    }
}

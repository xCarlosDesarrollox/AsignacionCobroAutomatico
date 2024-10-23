using System.ComponentModel.DataAnnotations;

namespace AsignacionCobroAutomatico.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Dpi { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress()]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Tipo Notificación")]
        public int TipoNotificacionId { get; set; }
    }
}

using AsignacionCobroAutomatico.Models;
using AsignacionCobroAutomatico.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsignacionCobroAutomatico.Controllers
{
    public class PagoAutomatizadosController: Controller
    {
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IRepositorioServiciosFijos repositorioServiciosFijos;
        private readonly IRepositorioPagoAutomatizado repositorioPagoAutomatizado;
        private readonly IRepositorioTarjeta repositorioTarjeta;

        public PagoAutomatizadosController(IRepositorioClientes repositorioClientes, IRepositorioServiciosFijos repositorioServiciosFijos,
            IRepositorioPagoAutomatizado repositorioPagoAutomatizado, IRepositorioTarjeta repositorioTarjeta) 
        {
            this.repositorioClientes = repositorioClientes;
            this.repositorioServiciosFijos = repositorioServiciosFijos;
            this.repositorioPagoAutomatizado = repositorioPagoAutomatizado;
            this.repositorioTarjeta = repositorioTarjeta;
        }
        public async Task<IActionResult> Index(int clienteId)
        {
            var modelo = await repositorioPagoAutomatizado.ListarDatoPorCliente(clienteId);
            var nombre = modelo.FirstOrDefault()?.NombreCliente;
            var notificacion = modelo.FirstOrDefault()?.NombreTipoNotificación;
            ViewBag.Notificacion = notificacion;
            ViewBag.NombreCliente = nombre;
            return View(modelo);
        }
        [HttpGet]
        public async Task<IActionResult> Asignar(string Dpi) 
        {
            var cliente = await repositorioClientes.BuscarPorDpi(Dpi);
            var modelo = new PagoAutomatizadoPrimeraVezViewModel
            {
                cliente = cliente,
                ServicioFijo = await ObtenerServicios(),
                TipoTarjeta = await ObtenerTipoTarjeta()
            };
            return View("AsignarServicio",modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Asignar(PagoAutomatizadoPrimeraVezViewModel modelo)
        {
            int idEmpresa = 0;
            var cliente = await repositorioClientes.BuscarPorDpi(modelo.cliente.Dpi);
            if(cliente is null)
            {
                return RedirectToAction("NoEncontrado","Home");
            }
            var empresa = await repositorioTarjeta.ListarEmpresa();
            var empresaEncontrada = empresa.FirstOrDefault(x => x.Descripcion.Equals(modelo.EmpresaNombre));
            if (empresaEncontrada != null) 
            {
                idEmpresa = empresaEncontrada.Id;
            }
            modelo.tarjeta.ClienteId = cliente.Id;
            modelo.tarjeta.EmpresaEmisoraId = idEmpresa;
            await repositorioTarjeta.AgregarTarjeta(modelo.tarjeta);
            
            foreach (var servicio in modelo.ServiciosSeleccionados) 
            {
                decimal monto = await repositorioServiciosFijos.ObtenerPrecio(servicio);
                PagoAutomatizado pago = new PagoAutomatizado()
                {
                    ClienteId = cliente.Id,
                    TarjetaId = modelo.tarjeta.Id,
                    ServicioId = int.Parse(servicio.ToString()),
                    Monto = monto,
                    FechaPago = DateTime.Now,
                    EstatusId = 1
                };
                int id = await repositorioPagoAutomatizado.Crear(pago);
                string numero = NumeroReferenciaServicio.AsignarNumeroReferencia(servicio);
                string NumRef = numero + Convert.ToString(id);
                await repositorioPagoAutomatizado.ActualizarNoRef(id,NumRef);
            }
            return RedirectToAction("Index", new { clienteId = cliente.Id });
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerServicios()
        {
            var servicio = await repositorioServiciosFijos.ListarServicio();
            return servicio.Select(x => new SelectListItem(x.Descripcion, x.Id.ToString()));
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTipoTarjeta() 
        {
            var modelo = await repositorioTarjeta.ListarTipo();
            return modelo.Select(x => new SelectListItem(x.Descripcion, x.Id.ToString()));
        }
    }
}

using AsignacionCobroAutomatico.Models;
using AsignacionCobroAutomatico.Servicios;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsignacionCobroAutomatico.Controllers
{
    public class ClientesController: Controller
    {
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IMapper mapper;

        public ClientesController(IRepositorioClientes repositorioClientes, IMapper mapper) {
            this.repositorioClientes = repositorioClientes;
            this.mapper = mapper;
        } 
        public async Task<IActionResult> Index() 
        { 
            var cliente = await repositorioClientes.ListarCliente();
            return View(cliente); 
        }
        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            var modelo = new ClienteViewModel();
            modelo.notificacion = await ObtenerNotificaciones();

            return View(modelo); 
        }
        [HttpPost]
        public async Task<IActionResult> Crear(ClienteViewModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }
            var nuevoCliente = cliente.Dpi; 
            await repositorioClientes.Crear(cliente);
            return RedirectToAction("Index","PagoAutomatizado", new { nuevoCliente});
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await repositorioClientes.BuscarCliente(id);
            if(cliente is null)
            {
                return RedirectToAction("Error", "Home");
            }
            var modelo = mapper.Map<ClienteViewModel>(cliente);
            modelo.notificacion = await ObtenerNotificaciones();
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(ClienteViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var cliente = mapper.Map<Cliente>(modelo);
            await repositorioClientes.Actualizar(cliente);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var cliente = await repositorioClientes.BuscarCliente(id);
            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarCliente(int id)
        {
            var cliente = await repositorioClientes.BuscarCliente(id);
            if (cliente == null)
            {
                return RedirectToAction("Error", "Home");
            }
            await repositorioClientes.Eliminar(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerNotificaciones()
        {
            var notificacion = await repositorioClientes.ObtenerNotificacion();
            return notificacion.Select(x => new SelectListItem(x.Descripcion, x.Id.ToString()));
        }
    }
}

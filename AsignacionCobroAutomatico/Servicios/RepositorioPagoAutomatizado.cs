using AsignacionCobroAutomatico.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AsignacionCobroAutomatico.Servicios
{
    public interface IRepositorioPagoAutomatizado
    {
        Task ActualizarNoRef(int id, string numRef);
        Task<int> Crear(PagoAutomatizado pago);
        Task<IEnumerable<PagoAutomatizadoViewModel>> ListarDatoPorCliente(int clienteId);
    }
    public class RepositorioPagoAutomatizado: IRepositorioPagoAutomatizado
    {
        private readonly string connectionString;
        public RepositorioPagoAutomatizado(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<PagoAutomatizadoViewModel>> ListarDatoPorCliente(int clienteId) 
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryAsync<PagoAutomatizadoViewModel>(@"
                    SELECT pagoAuto.Id,
                    cliente.Nombre AS NombreCliente, 
                    servicio.Descripcion AS NombreServicio, 
                    pagoAuto.Monto, 
                    pagoAuto.NumeroReferencia, 
                    notificacion.Descripcion AS NombreTipoNotificación 
                    FROM PagoAutomatizado pagoAuto 
                    JOIN Servicios servicio ON pagoAuto.ServicioId = servicio.Id
                    JOIN Clientes cliente ON pagoAuto.ClienteId = cliente.Id
                    JOIN TipoNotificaciones notificacion ON cliente.TipoNotificacionId = notificacion.Id
                    WHERE pagoAuto.ClienteId = @clienteId", new { clienteId});
        }
        public async Task<IEnumerable<PagoAutomatizadoViewModel>> ListarDatos()
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryAsync<PagoAutomatizadoViewModel>(@"SELECT * FROM PagoAutomatizado");
        }

        public async Task<int> Crear(PagoAutomatizado pago) 
        {
            using var conn = new SqlConnection(connectionString);
            var id = await conn.QuerySingleAsync<int>(@"INSERT INTO 
                    PagoAutomatizado(ClienteId, TarjetaId, ServicioId, Monto,FechaPago,EstatusId) 
                    VALUES(@ClienteId,@TarjetaId,@ServicioId,@Monto,@FechaPago,@EstatusId); SELECT SCOPE_IDENTITY();"
                    , new { pago.ClienteId,pago.TarjetaId,pago.ServicioId, pago.Monto, pago.FechaPago,pago.EstatusId});
            return pago.Id = id;
        }

        public async Task<int> ContarServicio(int servicioId) 
        {
            using var conn = new SqlConnection(connectionString);

            return await conn.ExecuteScalarAsync<int>(@"SELECT COUNT(*) 
                                        FROM PagoAutomatizado WHERE ServicioId = @ServicioId", new { servicioId});
        }

        public async Task ActualizarNoRef(int id, string numRef) 
        {
            using var conn = new SqlConnection(connectionString);
            await conn.ExecuteAsync("UPDATE PagoAutomatizado SET NumeroReferencia = @numRef WHERE Id = @id", new { id,numRef});
        }
    }
}

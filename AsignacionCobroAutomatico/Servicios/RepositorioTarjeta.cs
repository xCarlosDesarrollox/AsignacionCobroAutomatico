using AsignacionCobroAutomatico.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AsignacionCobroAutomatico.Servicios
{
    public interface IRepositorioTarjeta
    {
        Task ActualizarTarjeta(Tarjeta tarjeta);
        Task AgregarTarjeta(Tarjeta tarjeta);
        Task<IEnumerable<Empresa>> ListarEmpresa();
        Task<IEnumerable<TipoTarjeta>> ListarTipo();
        Task<Tarjeta> ObtenerTarjeta(int id);
    }
    public class RepositorioTarjeta : IRepositorioTarjeta
    {
        private readonly string connectionString;
        public RepositorioTarjeta(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task AgregarTarjeta(Tarjeta tarjeta)
        {
            using var connection = new SqlConnection(connectionString);
            var Id = await connection.QuerySingleAsync<int>(@"INSERT INTO Tarjetas (NumeroTarjeta, PinSeguridad,FechaExpiracion,TipoTarjetaId,EmpresaEmisoraId,ClienteId)
            VALUES(@NumeroTarjeta,@PinSeguridad,@FechaExpiracion,@TipoTarjetaId,@EmpresaEmisoraId,@ClienteId);
            SELECT SCOPE_IDENTITY()", tarjeta);
            tarjeta.Id = Id;
        }
        public async Task ActualizarTarjeta(Tarjeta tarjeta) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Tarjetas 
            SET NumeroTarjeta=@NumeroTarjeta, PinSeguridad=@PinSeguridad, FechaExpiracion=@FechaExpiracion, TipoTarjetaId=@TipoTarjetaId, 
            EmpresaEmisoraId=@EmpresaEmisoraId WHERE ClienteId=@ClienteId", tarjeta);
        }

        public async Task<Tarjeta> ObtenerTarjeta(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Tarjeta>("SELECT * FROM Tarjetas WHERE ClienteId = @id", new { id });
        }

        public async Task<IEnumerable<Empresa>> ListarEmpresa() 
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryAsync<Empresa>(@"SELECT * FROM EmpresaEmisora");
        }

        public async Task<IEnumerable<TipoTarjeta>> ListarTipo() 
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryAsync<TipoTarjeta>(@"SELECT * FROM TipoTarjetas");
        }

    }
}

using AsignacionCobroAutomatico.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AsignacionCobroAutomatico.Servicios
{
    public interface IRepositorioTarjeta
    {
        Task AgregarTarjeta(Tarjeta tarjeta);
        Task<IEnumerable<Empresa>> ListarEmpresa();
        Task<IEnumerable<TipoTarjeta>> ListarTipo();
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

using AsignacionCobroAutomatico.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AsignacionCobroAutomatico.Servicios
{
    public interface IRepositorioServiciosFijos
    {
        Task<IEnumerable<ServicioFijo>> ListarServicio();
        Task<decimal> ObtenerPrecio(int id);
    }
    public class RepositorioServiciosFijos : IRepositorioServiciosFijos
    {
        private readonly string connectionString;
        public RepositorioServiciosFijos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ServicioFijo>> ListarServicio()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ServicioFijo>(@"SELECT * FROM Servicios");
        }

        public async Task<decimal> ObtenerPrecio(int id) 
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryFirstOrDefaultAsync<decimal>(@"SELECT Monto FROM Servicios WHERE Id = @id", new { id});
        }
    }
}

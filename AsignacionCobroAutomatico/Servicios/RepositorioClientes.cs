using AsignacionCobroAutomatico.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AsignacionCobroAutomatico.Servicios
{
    public interface IRepositorioClientes
    {
        Task Actualizar(Cliente cliente);
        Task<Cliente> BuscarCliente(int id);
        Task<Cliente> BuscarPorDpi(string dpi);
        Task Crear(ClienteViewModel cliente);
        Task Eliminar(int id);
        Task<IEnumerable<ClienteActualizarViewModel>> ListarCliente();
        Task<IEnumerable<TipoNotificacion>> ObtenerNotificacion();
    }
    public class RepositorioClientes: IRepositorioClientes
    {
        private readonly string connectionString;
        public RepositorioClientes(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(ClienteViewModel cliente)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                $@"INSERT INTO Clientes (Dpi, Nombre, Apellido, Direccion, Telefono, Email, TipoNotificacionId) 
                VALUES(@Dpi, @Nombre, @Apellido, @Direccion, @Telefono, @Email, @TipoNotificacionId); SELECT SCOPE_IDENTITY()", cliente);

            cliente.Id = id;
        }
        public async Task<IEnumerable<TipoNotificacion>> ObtenerNotificacion()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoNotificacion>(@"SELECT * FROM TipoNotificaciones");
        }

        public async Task<IEnumerable<ClienteActualizarViewModel>> ListarCliente()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ClienteActualizarViewModel>(@"
                SELECT c.*, tn.Descripcion FROM Clientes c JOIN TipoNotificaciones tn ON c.TipoNotificacionId = tn.Id");                      
        }
        
        public async Task<Cliente> BuscarCliente(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cliente>($@"SELECT * FROM Clientes WHERE Id = @Id", new { id });
        }

        public async Task<Cliente> BuscarPorDpi(string dpi)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cliente>($@"SELECT * FROM Clientes WHERE Dpi = @dpi", new { dpi});
        }

        public async Task Actualizar(Cliente cliente)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync($@"
            UPDATE Clientes 
            SET Dpi = @Dpi, Nombre = @Nombre, Apellido = @Apellido, Direccion = @Direccion, 
            Telefono = @Telefono, Email = @Email, TipoNotificacionId = @TipoNotificacionId WHERE Id = @Id", cliente);
        }
        public async Task Eliminar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync($@"
            DELETE FROM Clientes WHERE Id = @Id", new {id});
        }

    }
}


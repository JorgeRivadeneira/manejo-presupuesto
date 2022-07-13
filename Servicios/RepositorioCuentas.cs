using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel cuentaEditar);
        Task Borrar(int id);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance)
                VALUES(@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"
                SELECT Cuentas.Id, Cuentas.Nombre, Balance, TC.Nombre AS TipoCuenta
                FROM Cuentas 
                INNER JOIN TiposCuentas TC
                ON TC.Id = Cuentas.TipoCuentaId
                WHERE TC.UsuarioId = @UsuarioId
                ORDER BY tc.Orden",  new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"
                SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, TipoCuentaId
                FROM Cuentas 
                INNER JOIN TiposCuentas TC
                ON TC.Id = Cuentas.TipoCuentaId
                WHERE TC.UsuarioId = @UsuarioId AND Cuentas.id = @Id
                ", new {id, usuarioId});
        }

        public async Task Actualizar(CuentaCreacionViewModel cuentaEditar)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                UPDATE cuentas
                SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                TipoCuentaId = @TipoCuentaId
                WHERE Id = @Id", cuentaEditar);
        }

        public async Task Borrar(int id)
        {
            var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Cuentas Where Id = @Id", new { id });

        }
    }
}

using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
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
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Cuentas(Nombre, TipoCuentaId, Descripcion, Balance) 
                                                          VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance)
                                                          SELECT SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;
        }


        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>($@"SELECT Cuentas.id, Cuentas.Nombre, Balance, TiposCuenta.Nombre as TipoCuenta
                                                          FROM Cuentas
                                                          INNER JOIN TiposCuenta 
                                                          ON TiposCuenta.id = Cuentas.TipoCuentaId
                                                          WHERE TiposCuenta.UsuarioId = @UsuarioId
                                                          ORDER BY TiposCuenta.Orden", new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            var connect = new SqlConnection(connectionString);
            return await connect.QueryFirstOrDefaultAsync<Cuenta>($@"SELECT Cuentas.id, Cuentas.Nombre, Balance, TiposCuenta.id
                                                          FROM Cuentas
                                                          INNER JOIN TiposCuenta 
                                                          ON TiposCuenta.id = Cuentas.TipoCuentaId
                                                          WHERE TiposCuenta.UsuarioId = @UsuarioId and Cuentas.Id = @Id",
                                                          new { id, usuarioId });
        }

        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            var connect = new SqlConnection(connectionString);
            await connect.ExecuteAsync($@"UPDATE CUENTAS
                                         SET Nombre = @Nombre, 
                                             Balance = @Balance, 
                                             Descripcion = @Descripcion, 
                                             TipoCuentaId = @TipoCuentaId
                                         WHERE Id = @Id", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync($@"DELETE Cuentas 
                                             WHERE Id = @Id",
                                             new {id});
        }
    }
}

    

public interface IRepositorioCuentas
{
    Task Actualizar(CuentaCreacionViewModel model);
    Task Borrar(int id);
    Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
    Task Crear(Cuenta cuenta);
    Task<Cuenta> ObtenerPorId(int id, int usuarioId);
}


using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioTipoCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
    }


    public class RepositorioTipoCuentas: IRepositorioTipoCuentas
    {
        private readonly string connectionString;
        public RepositorioTipoCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO TiposCuenta(Nombre, UsuarioId, Orden)
                                                    VALUES(@Nombre, @UsuarioId, 0);
                                                    SELECT SCOPE_IDENTITY()",  tipoCuenta);
            tipoCuenta.id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@$"SELECT 1 FROM TiposCuenta 
                                                                        WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;", new { nombre, usuarioId });
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@$"SELECT id, Nombre, Orden 
                                                              FROM TiposCuenta
                                                              WHERE UsuarioId = @UsuarioId", new {usuarioId});

        }
    }
}

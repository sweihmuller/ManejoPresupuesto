using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioTipoCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
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
    }

}

namespace ManejoPresupuesto.Servicios

{
    public interface IServicioUsario
    {
        int ObtenerUsuarioId();
    }
    public class ServicioUsuario: IServicioUsario
    {
        public int ObtenerUsuarioId()
        {
            return 1;
        }
    }
}

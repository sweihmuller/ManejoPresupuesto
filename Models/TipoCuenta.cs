namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int id  { get; set; }
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}

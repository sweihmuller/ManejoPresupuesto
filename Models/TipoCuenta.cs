using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validaciones;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int id { get; set; }
        [Display(Name = "Nombre del tipo de cuenta")]
        [PrimeraLetraMaysucula]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}

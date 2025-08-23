using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validaciones;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta: IValidatableObject
    {
        public int id { get; set; }
        [Display(Name = "Nombre del tipo de cuenta")]
        [PrimeraLetraMaysucula]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Nombre) && Nombre.Any(char.IsDigit)) 
            {
                yield return new ValidationResult("El nombre no puede contener números.", new[] {nameof(Nombre)});
            }
        }
    }
}

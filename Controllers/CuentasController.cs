using System.Threading.Tasks;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController: Controller
    {
        private readonly IRepositorioTipoCuentas repositorioTipoCuentas;
        private readonly IServicioUsario servicioUsario;
        private readonly IRepositorioCuentas repositorioCuentas;

        public CuentasController(IRepositorioTipoCuentas repositorioTipoCuentas, IServicioUsario servicioUsario, IRepositorioCuentas repositorioCuentas)
        {
            this.repositorioTipoCuentas = repositorioTipoCuentas;
            this.servicioUsario = servicioUsario;
            this.repositorioCuentas = repositorioCuentas;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var cuentaConTipoCuenta = await repositorioCuentas.Buscar(usuarioId);

            var modelo = cuentaConTipoCuenta.GroupBy(x => x.TipoCuenta).Select(grupo => new IndiceCuentasViewModel
            {
                TipoCuenta = grupo.Key,
                Cuentas = grupo.AsEnumerable()
            }).ToList();

            return View(modelo);
        }

        public async Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = await ObtenerTiposCuenta(usuarioId);
            
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTipoCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if(!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuenta(usuarioId);
                return View(cuenta);
            }

            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");

        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuenta(int usuarioId)
        {
            var tiposCuenta = await repositorioTipoCuentas.Obtener(usuarioId);

            return tiposCuenta.Select(x => new SelectListItem(x.Nombre, x.id.ToString()));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = new CuentaCreacionViewModel()
            {
                Id = cuenta.Id,
                Nombre = cuenta.Nombre,
                TipoCuentaId = cuenta.TipoCuentaId,
                Descripcion = cuenta.Descripcion,
                Balance = cuenta.Balance,
            };

            modelo.TiposCuentas = await ObtenerTiposCuenta(usuarioId);
            return View(modelo); 
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var tipoCuenta = await repositorioCuentas.ObtenerPorId(cuentaEditar.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Actualizar(cuentaEditar);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Borrar(id);

            return RedirectToAction("Index");
        }
      
    }
}

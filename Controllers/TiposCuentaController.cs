using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentaController : Controller
    {
        private readonly IRepositorioTipoCuentas _repositorioTipoCuentas;
        private readonly IServicioUsario servicioUsario;

        public TiposCuentaController(IRepositorioTipoCuentas repositorioTipoCuentas, IServicioUsario servicioUsario)
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
            this.servicioUsario = servicioUsario;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            return View(tipoCuentas);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = servicioUsario.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await _repositorioTipoCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }
            await _repositorioTipoCuentas.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await _repositorioTipoCuentas.Existe(nombre, usuarioId);

            if (yaExisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var tipoCuentaExiste = await _repositorioTipoCuentas.ObtenerPorId(tipoCuenta.id, usuarioId);
            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _repositorioTipoCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tipoCuentas.Select(x => x.id);

            var idsTiposCuentaNoPerteneceAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if(idsTiposCuentaNoPerteneceAlUsuario.Count() > 0)
            {
                return Forbid();
            }

            var tiposCuentasOrdenado = ids.Select((valor, indice) => new TipoCuenta() { id = valor, Orden = indice + 1 }).AsEnumerable();

            await _repositorioTipoCuentas.Ordenar(tiposCuentasOrdenado);
            return Ok();
        }


    }
}

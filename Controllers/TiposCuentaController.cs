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

        public TiposCuentaController(IRepositorioTipoCuentas repositorioTipoCuentas) 
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = 1;
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
            if(!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = 1;
            var yaExisteTipoCuenta = await _repositorioTipoCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if(yaExisteTipoCuenta)
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
            var usuarioId = 1;
            var yaExisteTipoCuenta = await _repositorioTipoCuentas.Existe(nombre, usuarioId);

            if(yaExisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }

        
    }
}

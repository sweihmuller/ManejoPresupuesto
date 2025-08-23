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
            await _repositorioTipoCuentas.Crear(tipoCuenta);
            return View();
        }
    }
}

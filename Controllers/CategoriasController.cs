using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServicioUsario servicioUsario;

        public CategoriasController(IRepositorioCategorias repositorioCategorias, IServicioUsario servicioUsario)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.servicioUsario = servicioUsario;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var categorias = await repositorioCategorias.Obtener(usuarioId);

            return View(categorias);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.UsuarioId = servicioUsario.ObtenerUsuarioId();
            categoria.UsuarioId = usuarioId;

            await repositorioCategorias.Crear(categoria);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost] 
        public async Task<IActionResult> Editar(Categoria categoriaEditar)
        {
            if (!ModelState.IsValid) 
            {
                return View(categoriaEditar);
            }
            var usuarioId = servicioUsario.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(categoriaEditar.Id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            categoriaEditar.UsuarioId = usuarioId;

            await repositorioCategorias.Actualizar(categoriaEditar);

            return RedirectToAction("Index");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MeuProjeto.Models;

namespace MeuProjeto.Controllers
{
    public class MapaController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LocalizacaoModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LocalizacaoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The coordinates are available here.
            double latitude = model.Latitude!.Value;
            double longitude = model.Longitude!.Value;

            // TODO: Save the coordinates to your database if needed.

            TempData["Mensagem"] =
                $"Localização selecionada: {latitude:F6}, {longitude:F6}";

            return RedirectToAction(nameof(Index));
        }
    }
}
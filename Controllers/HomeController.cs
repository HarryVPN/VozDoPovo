using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VozDoPovo.Data;
using VozDoPovo.Models;

namespace VozDoPovo.Controllers
{
    public class HomeController : Controller
    {
        private readonly BancoContext _bancoContext;

        public HomeController(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public IActionResult Index()
        {
            List<DenunciaModel> denuncias = _bancoContext.Denuncias.ToList();
            return View(denuncias);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

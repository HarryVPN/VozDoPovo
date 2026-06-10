using VozDoPovo.Data;
using VozDoPovo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace VozDoPovo.Controllers
{
    public class DenunciaController : Controller
    {
        private readonly BancoContext _bancoContext;

        public DenunciaController(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public IActionResult CriarDenuncia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DenunciaCriada(DenunciaModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (model.arquivoImg != null)
            {
                string nomeArquivo = Path.GetRandomFileName() + Path.GetExtension(model.arquivoImg.FileName);

                using var stream = new FileStream(
                    Path.Combine("wwwroot/images", nomeArquivo),
                    FileMode.Create);

                model.imagem = nomeArquivo;

                await model.arquivoImg.CopyToAsync(stream);
            }

            model.arquivoImg = null;
            _bancoContext.Denuncias.Update(model);
            _bancoContext.SaveChanges();

            TempData[MeusIDs.Sucesso] = JsonSerializer.Serialize(
                new JsonDenuncia { id=model.id }
            );

            return RedirectToAction("Index", "Home");
        }

    }
}

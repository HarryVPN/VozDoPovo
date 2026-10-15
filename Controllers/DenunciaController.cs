using VozDoPovo.Data;
using VozDoPovo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.IO;

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
            return View(new DenunciaModel());
        }

        public IActionResult EditarDenuncia(int id)
        {
            DenunciaModel? denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia == null) { throw new System.Exception("Id de contato nulo"); }
            return View(denuncia);
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

            model.data = DateTime.Now;

            model.arquivoImg = null;
            _bancoContext.Denuncias.Update(model);
            _bancoContext.SaveChanges();

            TempData[MeusIDs.Sucesso] = JsonSerializer.Serialize(
                new JsonDenuncia { id = model.id }
            );

            return RedirectToAction("Index", "Home");
        }


        public void DeletarImagemPorNome(string nomeArquivo)
        {
            string arquivo = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                nomeArquivo
            );

            if (System.IO.File.Exists(arquivo))
            {
                System.IO.File.Delete(arquivo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DenunciaEditada(DenunciaModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (model.arquivoImg != null)
            {
                if (model.imagem != null)
                    DeletarImagemPorNome(model.imagem);

                string nomeArquivo = Path.GetRandomFileName() + Path.GetExtension(model.arquivoImg.FileName);

                using var stream = new FileStream(
                    Path.Combine("wwwroot/images", nomeArquivo),
                    FileMode.Create);

                model.imagem = nomeArquivo;

                await model.arquivoImg.CopyToAsync(stream);
            }

            model.data = DateTime.Now;

            model.arquivoImg = null;
            _bancoContext.Denuncias.Update(model);
            _bancoContext.SaveChanges();

            TempData[MeusIDs.Sucesso] = JsonSerializer.Serialize(
                new JsonDenuncia { id = model.id }
            );

            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        public async Task<IActionResult> DeletarDenuncia(int id)
        {
            var denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia != null)
            {
                if (denuncia.imagem != null)
                    DeletarImagemPorNome(denuncia.imagem);

                _bancoContext.Denuncias.Remove(denuncia);
                _bancoContext.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        private IActionResult LikeDislike(int id, string action, string controller, bool like)
        {
            DenunciaModel? denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia != null)
            {
                if (like)
                    denuncia.likes += 1;
                else
                    denuncia.dislikes += 1;

                _bancoContext.Update(denuncia);
                _bancoContext.SaveChanges();
            }
            //if (!string.IsNullOrEmpty(pagina) && Url.IsLocalUrl(pagina))
            //   return LocalRedirect(pagina);
            return RedirectToAction(action, controller, id);
        }

        [HttpPost]
        public IActionResult Like(int id, string action, string controller)
        {
            Console.WriteLine("pfv");
            return LikeDislike(id, action, controller, true);
        }
        
        [HttpPost]
        public IActionResult Dislike(int id, string action, string controller)
        {
            return LikeDislike(id, action, controller, false);
        }

        public IActionResult VerDenuncia(int id)
        {
            DenunciaModel? denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);
            if (denuncia != null)
                return View(denuncia);
            else
                return RedirectToAction();
        }
    }
}

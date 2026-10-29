using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using VozDoPovo.Data;
using VozDoPovo.Models;

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
        [HttpPost]
        public async Task<IActionResult> CriarDenuncia(DenunciaModel model)
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

            return RedirectToAction("Reddit", "Home");
        }


        public IActionResult EditarDenuncia(int id)
        {
            DenunciaModel? denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia == null) { throw new System.Exception("Id de contato nulo"); }
            return View(denuncia);
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
        public async Task<IActionResult> EditarDenuncia(DenunciaModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (model.arquivoImg != null)
            {
                if (model.imagem != null)
                    DeletarImagemPorNome(model.imagem);

                string nomeArquivo = Path.GetRandomFileName() + Path.GetExtension(model.arquivoImg.FileName);

                using var stream = new FileStream( Path.Combine("wwwroot/images",nomeArquivo), FileMode.Create);

                model.imagem = nomeArquivo;

                await model.arquivoImg.CopyToAsync(stream);
            }

            model.data = DateTime.Now;
            model.arquivada = false;
            //model.arquivoImg = null;

            _bancoContext.Denuncias.Update(model);
            _bancoContext.SaveChanges();

            TempData[MeusIDs.Sucesso] = JsonSerializer.Serialize(
                new JsonDenuncia { id = model.id }
            );

            return RedirectToAction("Reddit", "Home");
        }

        //[HttpGet]
        public async Task<IActionResult> DeletarDenuncia(int id)
        {
            var denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia != null)
            {
                //if (denuncia.imagem != null)
                //    DeletarImagemPorNome(denuncia.imagem);

                //_bancoContext.Denuncias.Remove(denuncia);
                //_bancoContext.SaveChanges();
                denuncia.arquivada = true;
                _bancoContext.Denuncias.Update(denuncia);
                _bancoContext.SaveChanges();
            }

            return RedirectToAction("Reddit", "Home");
        }

        private IActionResult LikeDislike(int id, string url, bool like)
        {
            DenunciaModel? denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia != null)
            {
                if (like)
                    denuncia.likes += 1;
                else
                    denuncia.dislikes += 1;

                _bancoContext.Denuncias.Update(denuncia);
                _bancoContext.SaveChanges();
            }

            return Redirect(url);
        }

        public IActionResult Like(int id, string a)
        {
            return LikeDislike(id, a, true);
        }

        public IActionResult Dislike(int id, string a)
        {
            return LikeDislike(id, a, false);
        }

        private DenunciaModel? AbrirPost(int id)
        {
            DenunciaModel? denuncia = _bancoContext.Denuncias.FirstOrDefault(x => x.id == id);

            if (denuncia != null)
            {
                denuncia.comentarios = _bancoContext.Comentarios.Where(x => x.idPost == id).ToList();
                denuncia.comments = denuncia.comentarios.Count();

                ViewBag.map = true;

                return denuncia;
            }
            return null;
        }

        public async Task<IActionResult> VerDenuncia(int id)
        {
            var denuncia = AbrirPost(id);
            return View(denuncia);
        }

        [HttpPost]
        public IActionResult Comentar(ComentarioModel comentario)
        {
            int idPost = comentario.id;
            var denuncia = AbrirPost(idPost);

            if (comentario.mensagem.Length > 0)
            {
                comentario.data = DateTime.Now;
                comentario.idPost = comentario.id;
                comentario.id = 0;

                _bancoContext.Comentarios.Add(comentario);
                _bancoContext.SaveChanges();
            }

            return RedirectToAction("VerDenuncia", "Denuncia", denuncia);
        }
    }
}
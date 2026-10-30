using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using VozDoPovo.Data;
using VozDoPovo.Models;
using System.Security.Principal;

namespace VozDoPovo.Controllers
{
    public class HomeController : Controller
    {
        private readonly BancoContext _bancoContext;

        public HomeController(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public PerfilModel? AcharPerfil(int id)
        {
            PerfilModel? perfil = _bancoContext.Perfis.Where(x => x.id == id).FirstOrDefault();
            return perfil;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(PerfilModel formulario)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            PerfilModel? perfil = _bancoContext.Perfis.Where(x => x.nome.ToLower() == formulario.nome.ToLower()).FirstOrDefault();

            if (perfil is null)
            {
                TempData["a"] = "Nome de perfil não existe";
                return RedirectToAction("Index", "Home");
            }
            else if (formulario.senha != perfil.senha)
            {
                TempData["a"] = "Senha incorreta";
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, perfil.nome),
                new Claim(ClaimTypes.Role, perfil.cargo.ToString()),
                new Claim(ClaimTypes.UserData, perfil.id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Reddit", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult CriarPerfil()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CriarPerfil(PerfilModel formulario)
        {
            if (!ModelState.IsValid)
                return View();

            if (_bancoContext.Perfis.Where(x => x.nome.ToLower()==formulario.nome.ToLower()).FirstOrDefault() != null )
            {
                TempData["a"] = "Este nome já está sendo utilizado";
                return RedirectToAction("CriarPerfil","Home");
            }

            formulario.cargo = IdCargos.Membro;
            formulario.ultimoLogin = DateTime.Now;
            formulario.criada = DateTime.Now;

            _bancoContext.Perfis.Add(formulario);
            _bancoContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Reddit()
        {
            List<DenunciaModel> denuncias = _bancoContext.Denuncias.Where(x => !x.arquivada).ToList();

            foreach (DenunciaModel denuncia in denuncias)
            {
                denuncia.comments = _bancoContext.Comentarios.Where(x => x.idPost == denuncia.id).Count();
                PerfilModel? perfil = _bancoContext.Perfis.Where(x => x.id == denuncia.perfil).FirstOrDefault();
                if (perfil != null)
                {
                    denuncia.nome = perfil.nome;
                }
            }
            ViewBag.teste = _bancoContext.Comentarios.FirstOrDefault();
            return View(denuncias);
        }

        public async Task<IActionResult> Lixeira()
        {
            List<DenunciaModel> denuncias = _bancoContext.Denuncias.Where(x => x.arquivada).ToList();

            foreach (DenunciaModel denuncia in denuncias)
            {
                denuncia.comments = _bancoContext.Comentarios.Where(x => x.idPost == denuncia.id).Count();
            }
            return View(denuncias);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

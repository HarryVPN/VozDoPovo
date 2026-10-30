using System.Security.Claims;

namespace VozDoPovo.Data
{
    public static class MeusIDs
    {
        public const string Sucesso = "a";
        public const string Falha = "b";
    }

    public enum IdCargos : byte
    {
        Membro = 1,
        Admin = 2
    }

    public static class ClaimsHelper
    {
        //public static string? GetUserName(this ClaimsPrincipal user)
        //{
        //    return user.Identity?.Name;
        //}

        //public static string? GetUserRole(this ClaimsPrincipal user)
        //{
        //    return user.FindFirst(ClaimTypes.Role)?.Value;
        //}

        public static bool EstaLogado(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated ?? false;
        }
    }

}

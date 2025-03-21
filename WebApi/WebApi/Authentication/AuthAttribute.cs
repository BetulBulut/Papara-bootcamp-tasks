using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApi.Services;

namespace WebApi.Authentication;
public class AuthAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
       if (!context.HttpContext.Request.Headers.TryGetValue("Username", out var username) ||
            !context.HttpContext.Request.Headers.TryGetValue("Password", out var password))
        {
           
            context.Result = new UnauthorizedResult();
            return;
        }

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
           
            context.Result = new UnauthorizedResult();
            return;
        }
        var authService = context.HttpContext.RequestServices.GetService<IAuthService>();
        var user = authService?.Authenticate(username, password);

        if (user == null)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
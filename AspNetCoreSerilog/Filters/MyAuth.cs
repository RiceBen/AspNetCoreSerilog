using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspNetCoreSerilog.Filters
{
    public class MyAuth : Attribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var authValue))
            {
                if (string.Equals(authValue.Scheme, JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            context.Result = new ForbidResult(JwtBearerDefaults.AuthenticationScheme);
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var authValue))
            {
                if (string.Equals(authValue.Scheme, JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.CompletedTask;
                }
            }

            context.Result = new ForbidResult(JwtBearerDefaults.AuthenticationScheme);
            return Task.CompletedTask;
        }
    }
}
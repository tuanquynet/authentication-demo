using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.BasicAuthentication.Middleware
{
    public class AuthenticationValidation : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationToken = context.HttpContext.Request.Headers["Authorization"].ToString();
                var token = authorizationToken.Replace("Basic", "").Trim();
                var userCredential = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var username = userCredential.Split(':')[0];
                var password = userCredential.Split(':')[1];
                if (username == "thanh" && password == "123456")
                {
                    return next();
                }
            }
            // otherwise, return unauthorize
            context.Result = new ObjectResult("Unauthorize")
            {
                StatusCode = 401
            };
            context.HttpContext.Response.Headers.Add("WWW-Authenticate", "Basic real=weather-api");
            return Task.CompletedTask;
        }
    }
}
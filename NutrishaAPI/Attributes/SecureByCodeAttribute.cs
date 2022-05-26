using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NutrishaAPI.Attributes
{
    public class SecureByCodeAttribute : ActionFilterAttribute
    {
        private readonly string _code;

        public SecureByCodeAttribute(string code = "99F9B78A-F997-46CF-9C28-5BA6BBD406C0")
        {
            _code = code.ToLower();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var code = context.HttpContext.Request.Query.FirstOrDefault(q =>
                q.Key.ToLower() == "code").Value;

            if (code.ToString().ToLower() != _code)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            base.OnActionExecuting(context);
        }
    }
}
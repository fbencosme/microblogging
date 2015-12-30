using System.Linq;
using MicroBlogging.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace MicroBlogging.Filters
{
    public class Authenticate : ActionFilterAttribute
    {
        [FromServices]
        public MicroBloggerContext DbContext { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!Authenticated(context.HttpContext.Session))
                context.Result = new HttpUnauthorizedResult();

            base.OnActionExecuting(context);
        }

        private bool Authenticated(ISession session)
        {
            return session.GetInt32("user").HasValue;
        }

    }
}
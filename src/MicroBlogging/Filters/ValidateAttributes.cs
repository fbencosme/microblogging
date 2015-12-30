using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace MicroBlogging.Filters
{
    public class ValidateAttributes : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            base.OnActionExecuting(context);
        }
    }
}
using System.Web.Mvc;

namespace wildrydes.net.Decorators
{
    public class Protected : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Logic to execute before the action method
            //System.Diagnostics.Debug.WriteLine($"Action '{filterContext.ActionDescriptor.ActionName}' is executing.");
            //base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.Result = new RedirectResult($"/User/Login?or={filterContext.HttpContext.Request.Url.PathAndQuery}");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Logic to execute after the action method
            System.Diagnostics.Debug.WriteLine($"Action '{filterContext.ActionDescriptor.ActionName}' executed.");
            base.OnActionExecuted(filterContext);
        }
    }
}
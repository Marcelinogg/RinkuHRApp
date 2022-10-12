using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RinkuHRApp.Filters;

public class CheckSelectedPayrollAtribute : Attribute, IActionFilter 
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        if (session.GetString("SelectedPayroll") == null)
        {
            ((Controller)context.Controller).TempData["UnselectedPayroll"] = "Favor de seleccionar una nómina";
            
            //Change the Result to point back to Home/PayrollSelect
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new 
            { 
                controller = "Home", 
                action = "PayrollSelect" 
            }));
        }
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
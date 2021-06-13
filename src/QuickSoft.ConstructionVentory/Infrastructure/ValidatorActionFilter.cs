using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuickSoft.ConstructionVentory.Infrastructure
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ModelState.IsValid) return;
            var result = new ContentResult();
            var errors = filterContext.ModelState.ToDictionary(valuePair => valuePair.Key, valuePair => valuePair.Value.Errors.Select(x => x.ErrorMessage).ToArray());

            var content = JsonSerializer.Serialize(new { errors });
            result.Content = content;
            result.ContentType = "application/json";

            filterContext.HttpContext.Response.StatusCode = 422; //unprocessable entity;
            filterContext.Result = result;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
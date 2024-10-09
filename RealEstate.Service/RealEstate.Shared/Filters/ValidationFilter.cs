using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using RealEstate.Shared.CustomEntities;

namespace RealEstate.Shared.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new CustomResponseResult(StatusCodes.Status400BadRequest, ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                    context.ModelState.Values.SelectMany(v => v.Errors, (e, error) => error.ErrorMessage).Aggregate(string.Empty, (current, next) => $"{current} - {next}"));
                return;
            }
            await next();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Shared.CustomEntities
{
    public class CustomResponseResult : JsonResult
    {
        public CustomResponseResult(int status, string message, object description) : base(new Response
        {
            Status = status,
            Message = message,
            Description = description
        })
        { }
    }
}

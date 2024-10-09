using System.Net;

namespace RealEstate.Shared.Exceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode Status { get; set; }
        public string DescriptionStatus { get; set; }

        public BusinessException()
        {
            Status = HttpStatusCode.BadRequest;
            DescriptionStatus = string.Empty;
        }

        public BusinessException(HttpStatusCode status, string descriptionStatus, string message) : base(message) 
        {
            Status = status;
            DescriptionStatus = descriptionStatus;            
        }
    }
}

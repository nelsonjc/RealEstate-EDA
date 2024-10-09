using System.Text.Json.Serialization;

namespace RealEstate.Producer.Requests
{
    public class PropertyImageCreationRequest
    {
        public string FileBase64 { get; set; }
        public bool Enable { get; set; }
    }
}

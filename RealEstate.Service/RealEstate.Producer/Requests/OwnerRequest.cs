namespace RealEstate.Producer.Requests
{
    public class OwnerRequest
    {
        public string Document { get; set; }
        public string Name { get; init; }
        public string Address { get; init; }
        public string Photo { get; init; }
        public DateTime Birthday { get; set; }
    }
}

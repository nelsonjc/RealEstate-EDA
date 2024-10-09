
namespace RealEstate.Shared.CustomEntities
{
    public record Owner
    {
        public Guid IdOwner { get; init; } = Guid.NewGuid();


        public string Document { get; set; }
        public string Name { get; init; }
        public string Address { get; init; }
        public string Photo { get; init; }
        public DateTime Birthday { get; set; }

        public Owner()
        {
            
        }

        public Owner(string document, string name, string address, string photo, DateTime birthday)
        {
            Document = document;
            Name = name;
            Address = address;
            Photo = photo;
            Birthday = birthday;
        }
    }
}

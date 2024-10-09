
namespace RealEstate.Shared.CustomEntities
{
    public class Property
    {
        public Property(Guid idProperty, string name, string address, decimal price, string codeInternal, int year, Guid idOwner, bool active)
        {
            IdProperty = idProperty;
            Name = name;
            Address = address;
            Price = price;
            CodeInternal = codeInternal;
            Year = year;
            IdOwner = idOwner;
            Active = active;
        }

        public Guid IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public Guid IdOwner { get ; set; } 
        public bool Active { get; set; }
        public Owner Owner { get; set; }
        public List<PropertyTrace> Traces { get; set; } = [];
        public List<PropertyImage> Images { get; set; } = [];
    }
}

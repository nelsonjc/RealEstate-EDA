namespace RealEstate.Database.Entities
{
    public class PropertyReport
    {
        public Guid Id { get; init; }
        public Guid IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public Guid IdOwner { get; set; }
        public bool Active { get; set; }
        public OwnerReport Owner { get; set; }
        public List<PropertyTraceReport> Traces { get; set; } = [];
        public List<PropertyImageReport> Images { get; set; } = [];
    }
}

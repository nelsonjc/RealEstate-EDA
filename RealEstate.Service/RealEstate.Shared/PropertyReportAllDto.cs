namespace RealEstate.Shared
{
    public class PropertyReportAllDto
    {
        public Guid Id { get; init; }
        public Guid IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public Guid IdOwner { get; set; }
        public string Owner { get; set; }
        public bool Active { get; set; }
        public int Traces { get; set; }
        public int Images { get; set; }
    }
}

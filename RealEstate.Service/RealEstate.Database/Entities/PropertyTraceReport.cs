namespace RealEstate.Database.Entities
{
    public class PropertyTraceReport
    {
        public Guid Id { get; init; }
        public Guid IdPorpertyTrace { get; set; }
        public Guid IdProperty { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public PropertyReport Property { get; set; }
    }
}

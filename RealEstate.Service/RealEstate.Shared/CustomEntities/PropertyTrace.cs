namespace RealEstate.Shared.CustomEntities
{
    public class PropertyTrace
    {
        public PropertyTrace(Guid idProperty, DateTime dateSale, string name, decimal value, decimal tax)
        {
            IdPorpertyTrace = Guid.NewGuid();
            IdProperty = idProperty;
            DateSale = dateSale;
            Name = name;
            Value = value;
            Tax = tax;
        }

        public Guid IdPorpertyTrace { get; set; }
        public Guid IdProperty { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}

namespace RealEstate.Database.Entities
{
    public class OwnerReport
    {
        public Guid Id { get; init; }
        public Guid IdOwner { get; init; }
        public string Name { get; init; }
        public string Address { get; init; }
        public string Photo { get; init; }
        public DateTime Birthday { get; set; }
        public List<PropertyReport> Properties { get; set; }
    }
}

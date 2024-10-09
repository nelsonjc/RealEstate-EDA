namespace RealEstate.Database.Entities
{
    public class PropertyImageReport
    {
        public Guid Id { get; init; }
        public Guid IdPropertyImage { get; set; }
        public Guid IdProperty { get; set; }
        public string FileUrl { get; set; }
        public bool Enable { get; set; }
        public PropertyReport Property { get; set; }
    }
}

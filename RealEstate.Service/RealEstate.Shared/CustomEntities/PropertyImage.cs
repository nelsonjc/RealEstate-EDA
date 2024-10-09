namespace RealEstate.Shared.CustomEntities
{
    public class PropertyImage
    {
        public PropertyImage(Guid idProperty, string fileBase64, bool enable)
        {
            IdPropertyImage = Guid.NewGuid();
            IdProperty = idProperty;
            FileBase64 = fileBase64;
            Enable = enable;
        }

        public Guid IdPropertyImage { get; set; }
        public Guid IdProperty { get; set; }
        public string FileBase64 { get; set; }
        public bool Enable { get; set; }
    }
}

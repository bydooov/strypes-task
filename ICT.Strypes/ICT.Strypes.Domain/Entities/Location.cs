namespace ICT.Strypes.Domain.Entities
{
    public class Location
    {
        public string? LocationId { get; set; }
        public LocationType Type { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<ChargePoint>? ChargePoints { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

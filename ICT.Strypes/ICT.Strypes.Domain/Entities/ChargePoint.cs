namespace ICT.Strypes.Domain.Entities
{
    public class ChargePoint
    {
        public string? ChargePointId { get; set; }
        public string? LocationId { get; set; }
        public Location? Location { get; set; }
        public ChargePointStatus Status { get; set; }
        public string? FloorLevel { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

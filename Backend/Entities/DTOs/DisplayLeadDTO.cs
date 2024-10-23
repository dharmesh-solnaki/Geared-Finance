namespace Entities.DTOs
{
    public class DisplayLeadDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string? Vendor { get; set; }
        public string? LeadSource { get; set; }
        public string? ClientName { get; set; }
        public string? ContactPerson { get; set; }
        public decimal? Amount { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? GafSalesRep { get; set; }
        public string? Substage { get; set; }
        public DateTime? LastActivity { get; set; }
        public string CreatedBy { get; set; }
    }
}

namespace Entities.UtilityModels
{
    public class SqlSearchParams
    {
        public int PipelineType { get; set; }
        public int LeadType { get; set; }
        public int[] OwnerIds { get; set; } = Array.Empty<int>();
        public int VendorId { get; set; }
        public int[] VendorSalesIds { get; set; } = Array.Empty<int>();
        public string RecordType { get; set; } = null!;
        public int UserId { get; set; }
        public int ListType { get; set; }

    }
}

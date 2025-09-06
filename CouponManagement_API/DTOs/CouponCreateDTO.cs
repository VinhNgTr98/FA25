namespace CouponManagement_API.DTOs
{
    public class CouponCreateDTO
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? MaxUsage { get; set; }
        public decimal? MinOrderAmount { get; set; }
    }
}

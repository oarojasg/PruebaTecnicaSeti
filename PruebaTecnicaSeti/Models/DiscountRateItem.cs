using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaSeti.Models
{
    public class DiscountRateItem
    {
        [Key]
        public int DiscountRateId { get; set; }
        public int DiscountRatePercentage { get; set; }
        public int PeriodId { get; set; }
        public PeriodItem PeriodItem { get; set; }
    }
}

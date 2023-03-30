using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaSeti.Models
{
    public class PeriodItem
    {
        [Key]
        public int PeriodId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public ProjectMovementItem? projectMovement { get; set; }
        public DiscountRateItem discountRate { get; set; }
    }
}

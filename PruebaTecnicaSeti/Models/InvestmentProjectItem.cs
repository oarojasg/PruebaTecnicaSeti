using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaSeti.Models
{
    public class InvestmentProjectItem
    {
        [Key]
        public int ProjectId { get; set; }
        public string? ProjectCode { get; set; }
        public string? ProjectDescription { get; set; }
        public int SubsectorId { get; set; }
        public int BrokerId { get; set; }
        public int InvestmentRegionId { get; set; }
        public Decimal InvestmentAmount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaSeti.Models
{
    public class BrokerItem
    {
        [Key]
        public int BrokerId { get; set; }
        public string? BrokerName { get; set; }
        public string? BrokerCode { get; set; }
        public int LocationRegionId { get; set; }
    }
}

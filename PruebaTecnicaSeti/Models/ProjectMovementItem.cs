using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaSeti.Models
{
    public class ProjectMovementItem
    {
        [Key]
        public int MovementId { get; set; }
        public Decimal MovementAmount { get; set; }
        public int PeriodId { get; set; }
        public int ProjectId { get; set; }
        public PeriodItem Period { get; set; }
    }
}

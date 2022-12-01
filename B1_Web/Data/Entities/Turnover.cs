using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B1_Web.Data.Entities
{
    public class Turnover
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TurnoverId { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public int BusinessAccountId { get; set; }
        public BusinessAccount BusinessAccount { get; set; }
    }
}

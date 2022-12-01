using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B1_Web.Data.Entities
{
    public class OutgoingBalance
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OutgoingBalanceId { get; set; }
        public double Active { get; set; }
        public double Passive { get; set; }
        public int BusinessAccountId { get; set; }
        public BusinessAccount BusinessAccount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B1_Web.Data.Entities;

public class IncomingBalance
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int IncomingBalanceId { get; set; }
    public double Active { get; set; }
    public double Passive { get; set; }
    public int BusinessAccountId { get; set; }
    public BusinessAccount BusinessAccount { get; set; }
}
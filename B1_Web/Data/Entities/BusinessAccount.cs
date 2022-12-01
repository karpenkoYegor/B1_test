using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace B1_Web.Data.Entities;

public class BusinessAccount
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int BalanceId { get; set; }
    public IncomingBalance IncomingBalance { get; set; }
    public Turnover Turnover { get; set; }
    public OutgoingBalance OutgoingBalance { get; set; }
    public int TypeBusinessAccountId { get; set; } 
    public TypeBusinessAccount TypeBusinessAccount { get; set; }

    public int FileNameId { get; set; }
    public DataFile FileName { get; set; }
}
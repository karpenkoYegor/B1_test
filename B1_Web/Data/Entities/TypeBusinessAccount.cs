using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B1_Web.Data.Entities;

public class TypeBusinessAccount
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<BusinessAccount> BusinessAccounts { get; set; }
}
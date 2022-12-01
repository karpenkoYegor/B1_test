using B1_Web.Data.Entities;

namespace B1_Web.Models;

public class DataRowModel
{
    public int BalanceId { get; set; }
    public double IncomingBalanceActive { get; set; }
    public double IncomingBalancePassive { get; set; }
    public double TurnoverDebit { get; set; }
    public double TurnoverCredit { get; set; }
    public double OutgoingBalanceActive { get; set; }
    public double OutgoingBalancePassive { get; set; }
}
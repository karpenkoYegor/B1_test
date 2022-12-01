namespace B1_Web.Models;

public class DataGroupModel
{
    public IEnumerable<DataRowModel> Rows { get; set; }
    public int GroupId { get; set; }
    public double IncomingBalanceActiveSum { get; set; }
    public double IncomingBalancePassiveSum { get; set; }
    public double TurnoverDebitSum { get; set; }
    public double TurnoverCreditSum { get; set; }
    public double OutgoingBalanceActiveSum { get; set; }
    public double OutgoingBalancePassiveSum { get; set; }
}
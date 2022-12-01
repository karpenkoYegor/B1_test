namespace B1_Web.Models;

public class DataFileModel
{
    public string FileName { get; set; }
    public IEnumerable<DataClassModel> Classes { get; set; }
    public double IncomingBalanceActiveFileSum { get; set; }
    public double IncomingBalancePassiveFileSum { get; set; }
    public double TurnoverDebitFileSum { get; set; }
    public double TurnoverCreditFileSum { get; set; }
    public double OutgoingBalanceActiveFileSum { get; set; }
    public double OutgoingBalancePassiveFileSum { get; set; }
}
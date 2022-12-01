namespace B1_Web.Models;

public class DataClassModel
{
    public IEnumerable<DataGroupModel> Groups { get; set; }
    public int ClassId { get; set; }
    public string Name { get; set; }
    public double IncomingBalanceActiveClassSum { get; set; }
    public double IncomingBalancePassiveClassSum { get; set; }
    public double TurnoverDebitClassSum { get; set; }
    public double TurnoverCreditClassSum { get; set; }
    public double OutgoingBalanceActiveClassSum { get; set; }
    public double OutgoingBalancePassiveClassSum { get; set; }

}
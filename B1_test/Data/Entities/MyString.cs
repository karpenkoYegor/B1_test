using System.ComponentModel.DataAnnotations;

namespace B1_test.Data.Entities;

public class MyString
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string EngSymbols { get; set; }
    public string RusSymbols { get; set; }
    public long EvenNumber { get; set; }
    public double FracNumber { get; set; }
}
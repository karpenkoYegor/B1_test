using B1_Web.Data.Entities;

namespace B1_Web.Models;

public class IndexViewModel
{
    public IEnumerable<string> FileNames { get; set; }
    public DataFileModel DataFileModel { get; set; }
}
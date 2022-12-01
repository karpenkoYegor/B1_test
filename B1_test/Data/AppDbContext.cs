using B1_test.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace B1_test.Data;

public class AppDbContext : DbContext
{
    public DbSet<MyString> MyStrings => Set<MyString>();
    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=B1Db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    }
}
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using B1_Web.Data.Entities;

namespace B1_Web.Data;

public class AppDbContext : DbContext
{
    public DbSet<BusinessAccount> BusinessAccounts => Set<BusinessAccount>();
    public DbSet<IncomingBalance> IncomingBalances => Set<IncomingBalance>();
    public DbSet<Turnover> Turnovers => Set<Turnover>();
    public DbSet<OutgoingBalance> OutgoingBalances => Set<OutgoingBalance>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Добавляем начальные данные классов аккаунтов
        modelBuilder.Entity<TypeBusinessAccount>().HasData(new TypeBusinessAccount[]
        {
            new TypeBusinessAccount { Id = 1, Name = "Денежные средства, драгоценные металлы и межбанковские операции" },
            new TypeBusinessAccount { Id = 2, Name = "Кредитные и иные активные операции с клиентами" },
            new TypeBusinessAccount { Id = 3, Name = "Счета по операциям клиентов" },
            new TypeBusinessAccount { Id = 4, Name = "Ценные бумаги" },
            new TypeBusinessAccount { Id = 5, Name = "Долгосрочные финансовые вложения в уставные фонды юридических лиц, основные средства и прочее имущество" },
            new TypeBusinessAccount { Id = 6, Name = "Прочие активы и прочие пассивы" },
            new TypeBusinessAccount { Id = 7, Name = "Собственный капитал банка" },
            new TypeBusinessAccount { Id = 8, Name = "Доходы банка" },
            new TypeBusinessAccount { Id = 9, Name = "Расходы банка" }
        });
    }
}
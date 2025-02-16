using FastMarketBackEnd.models;
using Microsoft.EntityFrameworkCore;



namespace FastMarketBackEnd.Data;

public  class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public ApplicationContext(DbContextOptions options): base(options)
    {
        // Database.EnsureCreated();
    }
    
    public DbSet<Role> Roles { get; set; }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=здесь_указывается_пароль_от_postgres");
    // }
}
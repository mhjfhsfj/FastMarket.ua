using FastMarketBackEnd.models;
using Microsoft.EntityFrameworkCore;



namespace FastMarketBackEnd.Data;

public  class ApplicationContext : DbContext
{
    
    public ApplicationContext(DbContextOptions options): base(options)
    {
        // Database.EnsureCreated();
    }
    
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=здесь_указывается_пароль_от_postgres");
    // }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<Moderation> Moderations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<PrimaryСategory> PrimaryСategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StatusModeration> StatusModerations { get; set; }
    public DbSet<StatusOsrder> StatusOsrders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Сategory> Categories { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<UserAgent> UsersAgent { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<PictureProduct> PictureProducts { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<UserPhoneCode> UserPhoneCodes { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    
}
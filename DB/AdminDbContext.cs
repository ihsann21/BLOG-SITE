using blogSitesi.Models;
using Microsoft.EntityFrameworkCore;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().HasData(
            new Admin
            {
                Id = 1,
                Email = "admin@example.com",
                Password = "password123" // Güvenlik için hashlenmiş şekilde olmalı
            }
        );
    }

}

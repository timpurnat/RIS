using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class SchoolContext : IdentityDbContext<ApplicationUser>
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Izdelek> Izdelki { get; set; }
        public DbSet<Znamka> Znamke { get; set; }
        public DbSet<Zvrst> Zvrsti { get; set; }
        

        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder); 
          modelBuilder.Entity<Izdelek>().ToTable("Izdelki");
          
          modelBuilder.Entity<Znamka>().ToTable("Znamke");
          modelBuilder.Entity<Zvrst>().ToTable("Zvrsti");

          modelBuilder.Entity<Rezervacija>().ToTable("Rezervacije");
          modelBuilder.Entity<Kategorija>().ToTable("Kategorija");
        }
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class ClientDBContext : DbContext
    {
        public ClientDBContext(DbContextOptions<ClientDBContext> options) : base(options) 
        {
                
        }

        public virtual DbSet<Client> Clients { get; set; }  
        public virtual DbSet<Address> Addresss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasIndex(e => e.Email)
                .IsUnique();


            modelBuilder.Entity<Address>()
            .HasOne(f => f.Client)
            .WithMany(p => p.AddressList)
            .HasForeignKey(f => f.ClientId);


            modelBuilder.Entity<Address>()
            .HasIndex(e => new { e.ClientId, e.Street }) 
            .IsUnique();
        }
    }
}

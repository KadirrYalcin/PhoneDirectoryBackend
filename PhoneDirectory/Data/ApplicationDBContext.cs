using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Models;

namespace PhoneDirectory.Data
{
    public class ApplicationDBContext:IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {
        }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="ADMIN"
                },
                 new IdentityRole
                {
                    Name="User",
                    NormalizedName="USER"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            modelBuilder.Entity<AppUser>()   
                 .HasMany(p => p.Persons)
                 .WithOne(m => m.Appuser)
                 .HasForeignKey(m => m.AppUserId);

           modelBuilder.Entity<Person>()
                .HasMany(p => p.PhoneNumber)
                .WithOne(pn => pn.Person)
                .HasForeignKey(d => d.PersonId);

            modelBuilder.Entity<Email>()
                .HasOne(p => p.Person)
                .WithOne(e => e.Email)
                .HasForeignKey<Email>(d => d.PersonId);

            modelBuilder.Entity<Address>()
                 .HasOne(p => p.Person)
                 .WithOne(e => e.Address)
                 .HasForeignKey<Address>(d => d.PersonId);

            modelBuilder.Entity<Photo>()
                 .HasOne(p => p.Person)
                 .WithOne(e => e.Photo)
                 .HasForeignKey<Photo>(d => d.PersonId);


        }

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using App.Models.Contacts;

namespace App.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //thiết lập thêm khi tạo ra

        }

        public DbSet<Contact> contact {set; get;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var e in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = e.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    e.SetTableName(tableName.Substring(6));
                }
            }
        }
    }
}
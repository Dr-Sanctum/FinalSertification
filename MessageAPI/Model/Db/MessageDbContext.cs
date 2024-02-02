using Microsoft.EntityFrameworkCore;
using System.IO;
namespace MessageAPI.Model.Db
{

     public partial class MessageDbContext : DbContext
    {
        private string _connectionString;
        public MessageDbContext() 
        { 

        }

        public MessageDbContext(string connectionString)
        {
            _connectionString = connectionString;

        }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_connectionString);
        }

        
                                                                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("messages_pkey");
                entity.ToTable("messages");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.EmailFrom).HasMaxLength(255).HasColumnName("from");
                entity.Property(e => e.EmailTo).HasMaxLength(255).HasColumnName("to");
                entity.Property(e => e.Unread).HasColumnName("unread");
                entity.Property(e => e.Text).HasMaxLength(700).HasColumnName("text");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("users_pkey");
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.ToTable("users");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
                entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Salt).HasColumnName("salt");
                entity.Property(e => e.RoleId).HasConversion<int>();
            });

            modelBuilder
                .Entity<Role>()
                .Property(e => e.RoleId)
                .HasConversion<int>();

            modelBuilder
                .Entity<Role>().HasData(
                Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(e => new Role()
                {
                    RoleId = e,
                    Email = e.ToString(),
                }));



            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

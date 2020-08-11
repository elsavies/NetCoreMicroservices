using Auth_Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Auth_Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        private readonly int maxKeyLength = 1024;
        public IConfigurationRoot _configuration;

        public ApplicationDbContext()
        {
            
        }              

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)       
        {
            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);                           
        }        

        //Asp.Net Core Identity Model Config
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity Tables
            builder.Entity<ApplicationUser>(b =>
            {                
                //Primary key
                b.HasKey(u => u.Id);

                //Indexes for "normalized" username and email, to allow efficient lookups

                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex").IsUnique();

                //Maps to the AspNetUsers table
                b.ToTable(name:"Users", schema:"dbo");

                //A concurrency token for use with the optimistic concurrency checking

                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                //Limit the size of columns to use efficient database types
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                //The relationships between User and other entity types
                //Note that these relationships are configured with no navigation properties

                //Each User can have many UserClaims
                b.HasMany(uc => uc.Claims).WithOne(e => e.User).HasForeignKey(uc => uc.UserId).IsRequired();

                //Each User can have many UserLogins
                b.HasMany(ul => ul.Logins).WithOne(e => e.User).HasForeignKey(ul => ul.UserId).IsRequired();

                //Each User can have many UserTokens
                b.HasMany(ut => ut.Tokens).WithOne(e => e.User).HasForeignKey(ut => ut.UserId).IsRequired();              
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                //Primary key
                b.HasKey(uc => uc.Id);

                //Maps to the AspNetUserClaims table
                b.ToTable(name:"UserClaims", schema: "dbo");
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                //Composite primary key consisting of the LoginProvider and the key to use
                //with that provider
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                //Limit the size of the composite key columns due to common DB restrictions
                b.Property(l => l.LoginProvider).HasMaxLength(128);
                b.Property(l => l.ProviderKey).HasMaxLength(128);

                //Maps to the AspNetUserLogins table
                b.ToTable(name:"UserLogins", schema: "dbo");
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                //Composite primary key consisting of the UserId, LoginProvider and Name
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                //Limit the size of the composite key columns due to common DB restrictions
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);

                //Maps to the AspNetUserTokens table
                b.ToTable(name:"UserTokens", schema: "dbo");
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable(name:"Roles", schema: "dbo");

                //Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();

                //Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims).WithOne(e => e.Role).HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.HasKey(x => new { x.UserId, x.RoleId });
              
                b.ToTable(name:"UserRoles", schema: "dbo");
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable(name:"RoleClaims", schema: "dbo");
            });

            #endregion          
        }

    }
}

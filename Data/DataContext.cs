using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Exercise.Data.Model;
using Microsoft.AspNetCore.Identity;
namespace Exercise.Data
{
    public class DataContext : IdentityDbContext<AppUser, Role, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { 
            LoadData(); 
        }

        public DbSet<Products> Products {get;set;}

        public DbSet<Images> Images {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Guid ADMIN_ID = Guid.NewGuid();
            Guid User = Guid.NewGuid();
            builder.Entity<Role>().HasData(new List<Role>
                {
                    new Role {
                        Id = ADMIN_ID,
                       Name = "Admin",
                        NormalizedName = "ADMIN"
                    }
                });
            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = ADMIN_ID,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "Admin@unosquare.com",
                    NormalizedEmail = "ADMIN@UNOSQUARE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "unosquareAdmin2020."),
                },
                new AppUser
                {
                    Id = User,
                    UserName = "Client",
                    NormalizedUserName = "Client",
                    Email = "Client@Client.com",
                    NormalizedEmail = "CLIENT@CLIENT.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "2020Client."),
                }
            );
            builder.Entity<UserRoles>().HasData(new List<UserRoles>{
                new UserRoles{
                    RoleId= ADMIN_ID,
                    UserId = ADMIN_ID
                }
            });
            builder.Entity<Products>().HasIndex(u => u.Id).IsUnique();
            builder.Entity<Products>().HasData(new List<Products>{
                new Products{
                    Id= 1,
                    Name= "Barbie Developer",
                    AgeRestriction = 12,
                    Price = 25.99m,
                    Company = "Mattel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris porta arcu arcu, vel varius enim ia"
                },
                new Products{
                    Id= 2,
                    Name= "xyc",
                    AgeRestriction = 4,
                    Price = 75.50m,
                    Company = "Marvel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris porta arcu arcu, vel varius enim ia"
                },
                new Products{
                    Id= 3,
                    Name= "abc",
                    AgeRestriction = 18,
                    Price = 99.99m,
                    Company = "Nintendo",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris porta arcu arcu, vel varius enim ia"
                }
            });
        }  

        public async void LoadData(){
            // i implement that method because inmemorydatabase cant configure seed on onmodelcreating
            if(!Products.Any()){
                List<Products> P = new List<Products>(){
                    new Products{
                        Id= 1,
                        Name= "Barbie Developer",
                        AgeRestriction = 12,
                        Price = 25.99m,
                        Company = "Mattel",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris porta arcu arcu, vel varius enim ia"
                    },
                    new Products{
                        Id= 2,
                        Name= "xyc",
                        AgeRestriction = 4,
                        Price = 75.50m,
                        Company = "Marvel",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris porta arcu arcu, vel varius enim ia"
                    },
                    new Products{
                        Id= 3,
                        Name= "abc",
                        AgeRestriction = 18,
                        Price = 99.99m,
                        Company = "Nintendo",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris porta arcu arcu, vel varius enim ia"
                    }
                };
                await Products.AddRangeAsync(P);
                await SaveChangesAsync();
                Guid ADMIN_ID = Guid.NewGuid();
                Guid User = Guid.NewGuid();
                var hasher = new PasswordHasher<AppUser>();
                List<AppUser> Users = new List<AppUser>(){
                    new AppUser
                    {
                        Id = ADMIN_ID,
                        UserName = "admin",
                        NormalizedUserName = "admin",
                        Email = "Admin@unosquare.com",
                        NormalizedEmail = "ADMIN@UNOSQUARE.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "unosquareAdmin2020."),
                    },
                    new AppUser
                    {
                        Id = User,
                        UserName = "Client",
                        NormalizedUserName = "Client",
                        Email = "Client@Client.com",
                        NormalizedEmail = "CLIENT@CLIENT.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "2020Client."),
                    }
                };
                List<Role> Roles = new List<Role>(){
                    new Role {
                        Id = ADMIN_ID,
                       Name = "Admin",
                        NormalizedName = "ADMIN"
                    }
                };
                List<UserRoles> userRoles = new List<UserRoles>(){
                    new UserRoles{
                        RoleId= ADMIN_ID,
                        UserId = ADMIN_ID
                    }
                };
                foreach(var i in Users){
                    await AddAsync(i);
                    await SaveChangesAsync();
                }
                foreach(var i in Roles){
                    await AddAsync(i);
                    await SaveChangesAsync();
                }
                foreach(var i in userRoles){
                    await AddAsync(i);
                    await SaveChangesAsync();
                }
                
            }
        }
    
    }
}
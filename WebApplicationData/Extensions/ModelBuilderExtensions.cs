using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplicationData.Enties;
using WebApplicationData.Enums;

namespace WebApplicationData.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "This is home page of WebApplication" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of WebApplication" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of WebApplication" }
               );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en", Name = "English", IsDefault = false });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = 0,
                    SortOrder = 1,
                    Status = Status.Active,
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = 0,
                     SortOrder = 2,
                     Status = Status.Active
                 }
                 );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                  new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Laptop", LanguageId = "vi", SeoAlias = "laptop", SeoDescription = "Laptop cho gaming", SeoTitle = "Laptop cho gaming" },
                  new CategoryTranslation() { Id = 2, CategoryId = 2, Name = "Máy tính bảng", LanguageId = "vi", SeoAlias = "may-tinh-bang", SeoDescription = "Máy tính nhỏ gọn tiện lợi", SeoTitle = "Máy tính nhỏ gọn tiện lợi" }
                  //new CategoryTranslation() { Id = 3, CategoryId = 3, Name = "Tablet", LanguageId = "vi", SeoAlias = "tabler", SeoDescription = "Tablet cho mọi người", SeoTitle = "Tablet cho mọi người" },
                  //new CategoryTranslation() { Id = 4, CategoryId = 4, Name = "Máy tính bàn", LanguageId = "vi", SeoAlias = "may-tinh-ban", SeoDescription = "Máy tính bàn văn phòng", SeoTitle = "Máy tính bàn văn phòng" }
                    );

            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               Id = 1,
               DateCreated = DateTime.Now,
               OriginalPrice = 100000,
               Price = 200000,
               Stock = 0,
               ViewCount = 0,
           },
           new Product()
           {
               Id = 2,
               DateCreated = DateTime.Now,
               OriginalPrice = 200000,
               Price = 400000,
               Stock = 1,
               ViewCount = 2,
           });
            modelBuilder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id = 1,
                     ProductId = 1,
                     Name = "Máy tính Laptop",
                     Brand = "SamSung",
                     LanguageId = "vi",
                     SeoAlias = "may-tinh-lap-top",
                     SeoDescription = "Máy tính laptop gaming",
                     SeoTitle = "Máy tính laptop gaming",
                     Details = "Máy tính laptop gaming",
                     Description = "Máy tính laptop gaming"
                 },
                  new ProductTranslation()
                  {
                      Id = 2,
                      ProductId = 2,
                      Name = "Máy tính Bảng",
                      Brand="Hp",
                      LanguageId = "vi",
                      SeoAlias = "may-tinh-bảng",
                      SeoDescription = "Máy tính bảng tiện lợi",
                      SeoTitle = "Máy tính bảng tiện lợi",
                      Details = "Máy tính bảng tiện lợi",
                      Description = "Máy tính bảng tiện lợi"
                  });

                    
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 },
                new ProductInCategory() { ProductId = 2, CategoryId = 2 }
                );

            // any guid
            //var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            //var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            //modelBuilder.Entity<AppRole>().HasData(new AppRole
            //{
            //    Id = roleId,
            //    Name = "admin",
            //    NormalizedName = "admin",
            //    Description = "Administrator role"
            //});

            //var hasher = new PasswordHasher<AppUser>();
            //modelBuilder.Entity<AppUser>().HasData(new AppUser
            //{
            //    Id = adminId,
            //    UserName = "admin",
            //    NormalizedUserName = "admin",
            //    Email = "tedu.international@gmail.com",
            //    NormalizedEmail = "tedu.international@gmail.com",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null, "Abcd1234$"),
            //    SecurityStamp = string.Empty,
            //    FirstName = "Toan",
            //    LastName = "Bach",
            //    Dob = new DateTime(2020, 01, 31)
            //});

            //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            //{
            //    RoleId = roleId,
            //    UserId = adminId
            //});

            modelBuilder.Entity<Slide>().HasData(
              new Slide() { Id = 1, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 1, Url = "#", Image = "/themes/images/carousel/1.png", Status = Status.Active },
              new Slide() { Id = 2, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 2, Url = "#", Image = "/themes/images/carousel/2.png", Status = Status.Active },
              new Slide() { Id = 3, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 3, Url = "#", Image = "/themes/images/carousel/3.png", Status = Status.Active },
              new Slide() { Id = 4, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 4, Url = "#", Image = "/themes/images/carousel/4.png", Status = Status.Active },
              new Slide() { Id = 5, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 5, Url = "#", Image = "/themes/images/carousel/5.png", Status = Status.Active },
              new Slide() { Id = 6, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 6, Url = "#", Image = "/themes/images/carousel/6.png", Status = Status.Active }
              );
        }
    }
    
}

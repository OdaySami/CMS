﻿using CMC.Core.Enums;
using CMC.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Data
{
    public static class DbSeeder
    {
        public static IHost SeedDb(this IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<CMCDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                context.SeedCategory().Wait();
                userManager.SeedUser().Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return webHost;
        }

        public static async Task SeedCategory(this CMCDbContext _db)
        {
            if (await _db.Categories.AnyAsync())
            {
                return;
            }

            var categoires = new List<Category>();

            var category = new Category();
            category.Name = "A1";
            category.CreateAt = DateTime.Now;

            var category2 = new Category();
            category2.Name = "A2";
            category2.CreateAt = DateTime.Now;

            categoires.Add(category);
            categoires.Add(category2);

            await _db.Categories.AddRangeAsync(categoires);
            await _db.SaveChangesAsync();
        }

        public static async Task SeedUser(this UserManager<User> userManger)
        {
            if (await userManger.Users.AnyAsync())
            {
                return;
            }
            var user = new User();
            user.FullName = "System Developer";
            user.UserName = "dev@gmail.com";
            user.Email = "dev@gmail.com";
            user.CreateAt = DateTime.Now;
           

            await userManger.CreateAsync(user, "Admin111$$");
        }

    }
}

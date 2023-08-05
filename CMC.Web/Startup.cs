 using CMC.Data;
using CMC.Data.Models;
using CMC.Infrastructure.AutoMapper;
using CMC.Infrastructure.Middlewares;
using CMC.Infrastructure.Services;
using CMC.Infrastructure.Services.Advertisements;
using CMC.Infrastructure.Services.Categories;
using CMC.Infrastructure.Services.Notifications;
using CMC.Infrastructure.Services.Posts;
using CMC.Infrastructure.Services.Tracks;
using CMC.Infrastructure.Services.Users;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CMC.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CMCDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 6;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<CMCDbContext>()


                .AddDefaultTokenProviders().AddDefaultUI();

                
            services.AddRazorPages();
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAdvertisementService, AdvertisementService>();
            services.AddTransient<ITrackService, TrackService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<INotificationService, NotificationService>();




            services.AddControllersWithViews();


           

            //     .AddDefaultUI()
            //    .AddDefaultTokenProviders();
        }

        private void AddDefaultTokenProviders()
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(opts => opts.UseMiddleware<ExceptionHandler>());

            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(env.WebRootPath, "cmcweb-132b7-firebase-adminsdk-rpklo-17f8835a00.json")),
                });
            }catch (Exception ex)
            {

            }

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

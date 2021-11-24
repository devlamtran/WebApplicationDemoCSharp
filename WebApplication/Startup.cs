using System;
using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.LocalizationResources;
using WebApplicationData.EF;
using WebApplicationData.Enties;
using WebApplicationLogic.Catalog.Categories;
using WebApplicationLogic.Catalog.Languages;
using WebApplicationLogic.Catalog.Products;
using WebApplicationLogic.Catalog.Roles;
using WebApplicationLogic.Catalog.Sales;
using WebApplicationLogic.Catalog.Slides;
using WebApplicationLogic.Catalog.Users;
using WebApplicationLogic.Catalog.Users.Dto;

namespace WebApplication
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
            services.AddHttpClient();
            var cultures = new[]
           {
                new CultureInfo("en"),
                new CultureInfo("vi"),
            };
            services.AddControllersWithViews().AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
                {
                    // When using all the culture providers, the localization process will
                    // check all available culture providers in order to detect the request culture.
                    // If the request culture is found it will stop checking and do localization accordingly.
                    // If the request culture is not found it will check the next provider by order.
                    // If no culture is detected the default culture will be used.

                    // Checking order for request culture:
                    // 1) RouteSegmentCultureProvider
                    //      e.g. http://localhost:1234/tr
                    // 2) QueryStringCultureProvider
                    //      e.g. http://localhost:1234/?culture=tr
                    // 3) CookieCultureProvider
                    //      Determines the culture information for a request via the value of a cookie.
                    // 4) AcceptedLanguageHeaderRequestCultureProvider
                    //      Determines the culture information for a request via the value of the Accept-Language header.
                    //      See the browsers language settings

                    // Uncomment and set to true to use only route culture provider
                    ops.UseAllCultureProviders = false;
                    ops.ResourcesPath = "LocalizationResources";
                    ops.RequestLocalizationOptions = o =>
                    {
                        o.SupportedCultures = cultures;
                        o.SupportedUICultures = cultures;
                        o.DefaultRequestCulture = new RequestCulture("vi");
                    };
                }); ;

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 
                 options.LoginPath = "/Account/Login";
                 options.AccessDeniedPath = "/Account/AccessDenied";
             });
            


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //khai bao DI
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<ISlideService, SlideService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IStorageService,FileStorageService>();
            services.AddTransient<UserManager<User>, UserManager<User>>();
            services.AddTransient <SignInManager<User>, SignInManager<User>>();
            services.AddTransient<RoleManager<Role>, RoleManager<Role>>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<WebApplicationContext>().AddDefaultTokenProviders();
            services.AddDbContext<WebApplicationContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("WebSolutionDb")));

            services.AddControllers()
               .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("culture/Home/Error");
            }
            //app.UseMiddleware<AuthenticationMiddleware>();

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
                        
            app.UseSession();
            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Product Category En",
                    pattern: "{culture}/categories/{id?}", new {
                    controller = "Product",
                    action = "Category"
                    });

                endpoints.MapControllerRoute(
                   name: "Product Category Vn",
                   pattern: "{culture}/danh-muc/{id?}", new
                   {
                       controller = "Product",
                       action = "Category"
                   });
                

                endpoints.MapControllerRoute(
                   name: "Product Detail En",
                   pattern: "{culture}/products/{id?}", new
                   {
                       controller = "Product",
                       action = "Detail"
                   });

                endpoints.MapControllerRoute(
                  name: "Product Detail Vn",
                  pattern: "{culture}/san-pham/{id?}", new
                  {
                      controller = "Product",
                      action = "Detail"
                  });
                endpoints.MapControllerRoute(
                  name: "Product Detail Vn",
                  pattern: "{culture}/bill/{userName?}", new
                  {
                      controller = "Checkout",
                      action = "Bill"
                  });

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");

            });

            
        }
    }
}

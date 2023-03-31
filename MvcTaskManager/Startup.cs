using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcTaskManager.Identity;
using MvcTaskManager.ServiceContracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MvcTaskManager.JWT;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace MvcTaskManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json");
            Configuration = builder.Build();

        }


        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });

            });

            #region For Logger - Optional
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            #endregion

            services.AddMvc();

            services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MvcTaskManager")));

            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
            services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
            services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IUserService, UserService>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            services.AddScoped<ApplicationRoleStore>();
            services.AddScoped<ApplicationUserStore>();

            #region JWT Step 1
            var appSettingsSection = Configuration.GetSection("AppSetings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            var key = System.Text.Encoding.ASCII.GetBytes("ThisIsTheMostSecret");
            services.AddAuthentication(x =>
            {
                //x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; // enable for http request
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "XSRF-Cookie-TOKEN";
                options.HeaderName = "X-XSRF-TOKEN";
            });

            #endregion
        }


        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env) // async
        {
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseStaticFiles();

            #region Default User & Role

            /*
            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Create Admin Role
                if (!(await roleManager.RoleExistsAsync("admin")))
                {
                    var role = new ApplicationRole();
                    role.Name = "admin";
                    await roleManager.CreateAsync(role);
                }

                // Create Admin User
                if (await userManager.FindByNameAsync("Admin") == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = "Admin";
                    user.Email = "admin@admin.com";
                    var userPassword = "xNGb5As9^dZW13X^w";

                    var chkuser = await userManager.CreateAsync(user, userPassword);
                    if (chkuser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }

                // Create Employee Role
                if (!(await roleManager.RoleExistsAsync("employee")))
                {
                    var role = new ApplicationRole();
                    role.Name = "employee";
                    await roleManager.CreateAsync(role);
                }

                // Create Employee User
                if (await userManager.FindByNameAsync("employee") == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = "employee";
                    user.Email = "employee@employee.com";
                    var userPassword = "xNGb5As9^dZW13X^w";

                    var chkuser = await userManager.CreateAsync(user, userPassword);
                    if (chkuser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "employee");
                    }
                }
            }
            */

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);

            #region Routing and Endpoints
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #endregion
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using TripickServer.Models;
using TripickServer.Utils;

namespace TripickServer
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
            services
                .AddIdentity<AppUser, AppRole>(options => { options.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<TripickContext>()
                .AddTokenProvider(Constants.AppName, typeof(DataProtectorTokenProvider<AppUser>));

            services
                .AddDbContextPool<TripickContext>(options => options.UseNpgsql( // Remove "Pool" from "AddDbContextPool" when doing a migration
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.ProvideClientCertificatesCallback(clientCerts =>
                    {
                        var databaseCertificate = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + "/Resources/databaseCert.pfx";
                        var cert = new X509Certificate2(databaseCertificate, Configuration.GetValue<string>("Settings:databaseCertPassword"));
                        clientCerts.Add(cert);
                    }
            )));

            services.AddControllers(options => {
                //options.Filters.Add(new CheckAuthKeysAndConnect()); // To apply the attribute on all controllers
            }).AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddScoped<CheckAuthKeysAndConnect>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

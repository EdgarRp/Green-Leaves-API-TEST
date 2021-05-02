using GreenLeaves.Data;
using GreenLeaves.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GreenLeaves.Data.ServiceManager;
using GreenLeaves.Core.Account;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GreenLeaves.Core;
using GreenLeaves.Core.SendGrid;

namespace API {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; }
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        
        public void ConfigureServices( IServiceCollection services ) {
            //configure email unique and default token
            services.AddIdentity<UserModel, IdentityRole>( cfg => {
                cfg.User.RequireUniqueEmail = true;
            } ).AddEntityFrameworkStores<GreenLeaveContext>().AddDefaultTokenProviders();

            services.AddAuthentication().AddCookie().AddJwtBearer( cfg => {
                cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                    ValidIssuer = Configuration [ "Tokens:Issuer" ],
                    ValidAudience = Configuration [ "Tokens:Audience" ],
                    IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( Configuration [ "Tokens:Key" ] ) )
                };
            } );

            #region Conection
            string defaultConnectionString = Configuration.GetConnectionString( "DefaultConnection" );

            services.AddDbContext<GreenLeaveContext>( options => {
                options.UseSqlServer(
                        defaultConnectionString,
                        b => b.MigrationsAssembly( "GreenLeave.Api" ) );
            } );
            services.AddTransient<GreenLeaveSeeder>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region Transients
            services.AddTransient<AccountManager>();
            services.AddTransient<AccountCore>();

            services.AddTransient<ISendGrid, SendGridMail>();
            #endregion

            #region Cors
            services.AddCors( o => o.AddPolicy( "MyPolicy", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            } ) );
            services.AddMvc().AddNewtonsoftJson( 
                options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore );
            
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory ) {
            loggerFactory.AddFile( Configuration [ "LoggingPath" ] );

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors( "MyPolicy" );
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints( endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            } );
            

            
        }
    }
}

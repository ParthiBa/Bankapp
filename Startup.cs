using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;

namespace BankAPPWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;//get connection string value here and get it in dao
           // this.loggerfactory = logFactory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var serviceProvider = services.BuildServiceProvider();
            //var logger = serviceProvider.GetService<ILoggerFactory>();
            //ILoggerFactory loggerFactory = new LoggerFactory();
            //services.AddSingleton<ILoggerFactory>(loggerFactory);
            services.AddControllersWithViews();
            services.AddSession();
            /*var dao = new accountDAO.AccountDAO(logger);
            var bank=new Banks.Bank(dao);
            services.AddSingleton(dao);
            services.AddSingleton(bank);
            */
            services.AddLogging();
            //services.AddSingleton(typeof(ILogger<accountDAO.AccountDAO>),logger);
            services.AddTransient(typeof(accountDAO.AccountDAO));
            services.AddTransient(typeof(Banks.Bank));
            //services.AddTransient<ILogger<accountDAO.AccountDAO>>();
           // this.checkAndSeedDatabase(logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                 app.UseDeveloperExceptionPage();
            }
            app.UseSession();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void checkAndSeedDatabase(ILogger logger)
        {
            string script = File.ReadAllText("seed.sql");
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder(); ;
            connectionStringBuilder.DataSource = "./BankAPP_Data.db";
            connectionStringBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
            SqliteConnection conn = new SqliteConnection(connectionStringBuilder.ConnectionString);
            string query =@"SELECT count(name) FROM sqlite_master WHERE type='table' AND name='Customers';";
            SqliteCommand readcomm = new SqliteCommand(query, conn);
            conn.Open();
            int colstatus  = Convert.ToInt32(readcomm.ExecuteScalar());
            if(colstatus==0)
            {
                logger.LogError("Database is empty -------------------");
                logger.LogInformation($"length of script {script.Length}");
            }
            else 
            {
                logger.LogInformation($"Database has tables");
            }
            conn.Close();
        }
    }
}

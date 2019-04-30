using CustomerApi.Commands;
using CustomerApi.Events;
using CustomerApi.Models.Mongo;
using CustomerApi.Models.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace CustomerApi
{
	public class Startup
    {
		private IHostingEnvironment _env;

		public Startup(IHostingEnvironment env, IConfiguration config)
        {
			_env = env;
			Configuration = config;
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<CustomerSQLiteDatabaseContext>(options =>
				options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			services.AddMvc();

			services.AddTransient<CustomerMongoRepository>();
			services.AddTransient<CustomerSQLiteRepository>();

			services.AddTransient<AMQPEventPublisher>();
			services.AddSingleton<CustomerMessageListener>();

			services.AddScoped<ICommandHandler<Command>, CustomerCommandHandler>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, CustomerMessageListener messageListener)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<CustomerSQLiteDatabaseContext>();
				context.Database.EnsureCreated();
			}

			app.UseMvc();

			new Thread(() =>
			{
				messageListener.Start(_env.ContentRootPath);
			}).Start();
		}
    }
}

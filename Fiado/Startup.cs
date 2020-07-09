﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fiado.Models.AtendenteModelos;
using Fiado.Models.ClienteModelos;
using Fiado.Models.ContaModelos;
using Fiado.Models.NotaModelos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiado
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DB
            services.AddDbContextPool<FiadoContexto>(options =>
            {
                if (env.IsDevelopment())
                {
                    options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));

                }
                if (env.IsProduction())
                {
                    options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
                }

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Injecao de dependencias
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddScoped<INotaRepositorio, NotaRepositorio>();
            services.AddScoped<IContaRepositorio, ContaRepositorio>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Cliente}/{action=Lista}/{id?}");
            });
        }
    }
}

﻿using AutoMapper;
using fonenako.DatabaseContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace fonenako_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddFonenakoDbContext(Configuration)
                .AddFonenakoServices()
                .Configure<FunctionalSettings>(Configuration.GetSection(FunctionalSettings.Key))
                .AddSingleton(provider => new MapperConfiguration(cfg =>
                        {
                            cfg.AddProfile(new LeaseOfferProfile(Configuration.GetSection(FunctionalSettings.Key).GetValue<string>("PhotoUrlBase")));
                        }
                    ).CreateMapper());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FonenakoDbContext fonenakoDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler("/Error");
            app.UseRouting();
            app.UseCors(builder => builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region init fake data

            //FakeData.InitFakeData(fonenakoDbContext);

            #endregion
        }
    }
}

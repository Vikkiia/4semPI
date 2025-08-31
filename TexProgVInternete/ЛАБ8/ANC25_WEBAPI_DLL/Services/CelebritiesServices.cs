﻿using DAL_Celebrity_MSSQL;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ANC25_WEBAPI_DLL.Services
{
    public static class CelebritiesServices
    {
        public static WebApplicationBuilder AddCelebritiesServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CelebritiesConnection")));

            builder.Services.AddScoped<IRepository>(provider =>
            {
                var connectionString = builder.Configuration.GetConnectionString("CelebritiesConnection");
                return new Repository(AppConfig.ConnectionString);
            });

            builder.Services.AddSingleton<CountryCodes>(provider =>
            {
                var countriesPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "CountryCodes",
                    "iso3166-1-alpha2-country-codes.json");
                return CountryCodes.LoadFromFile(countriesPath);
            });

            builder.Services.AddSingleton<CelebrityTitles>(new CelebrityTitles
            {
                Title = "Celebrities Management System",
                Head = "Famous People Database",
                Copyright = $"© {DateTime.Now.Year} Celebrities App"
            });

            builder.Services.Configure<CelebritiesConfig>(builder.Configuration.GetSection("CelebritiesConfig"));

            return builder;
        }

        public static WebApplicationBuilder AddCelebritiesDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CelebritiesConnection")));

            builder.Services.AddScoped<IRepository>(provider =>
            {
                var context = provider.GetRequiredService<Context>();
                return new Repository(AppConfig.ConnectionString);
            });

            return builder;
        }
    }

    public class CountryCodes : IEnumerable<Country>
    {
        private readonly List<Country> _countries;

        private CountryCodes(List<Country> countries)
        {
            _countries = countries ?? throw new ArgumentNullException(nameof(countries));
        }

        public static CountryCodes LoadFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var countries = JsonSerializer.Deserialize<List<Country>>(json);
                return new CountryCodes(countries);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load country codes: {ex.Message}", ex);
            }
        }

        public IEnumerator<Country> GetEnumerator() => _countries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

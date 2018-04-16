using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace fluxo.DATA.Context
{
    //For EF Migrations only, to inject, use ServiceCollectionExtensions instead.
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public static string GetConnectionString(string key) {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
                
            return configuration.GetConnectionString(key);
        }

        public DataContext CreateDbContext(string[] args)
        {
            //TODO: Find a way to pass the connection string key through args or through environment vars
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite(DataContextFactory.GetConnectionString("SqliteConnection"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
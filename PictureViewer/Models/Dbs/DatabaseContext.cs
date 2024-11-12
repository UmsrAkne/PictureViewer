using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PictureViewer.Models.Dbs
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ExFileInfo> ImageFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string dbFileName = "db.sqlite";
            if (!File.Exists(dbFileName))
            {
                using var connection = new SqliteConnection($"Data Source={dbFileName}");
                connection.Open();
                connection.Close();
            }

            var connectionString = new SqliteConnectionStringBuilder { DataSource = dbFileName, }.ToString();
            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }
    }
}
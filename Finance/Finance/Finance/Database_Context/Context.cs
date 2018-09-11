using Finance.Model;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.IO;
using Xamarin.Forms;

namespace Finance.Database_Context
{
    class Context : DbContext
    {
        private const string DatabaseName = "database.db";

        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public Context()
        {
            Database.EnsureCreated();
            Transactions.CountAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string databasePath;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                    break;
                case Device.iOS:
                    Batteries.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
                    break;
                case Device.UWP:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>().Property(p => p.Date).HasColumnType("datetime2");
        }
    }
}

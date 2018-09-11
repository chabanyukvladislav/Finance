using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.Database_Context;
using Finance.Model;

namespace Finance.DatabaseConnector
{
    class DatabaseConnector : IDatabaseConnector
    {
        private static DatabaseConnector _databaseConnector;
        private static readonly object Locker = new object();

        private readonly Context _context;

        public static DatabaseConnector GetDatabaseConnector
        {
            get
            {
                if (_databaseConnector == null)
                {
                    lock (Locker)
                    {
                        if (_databaseConnector == null)
                            _databaseConnector = new DatabaseConnector();
                    }
                }

                return _databaseConnector;
            }
        }

        private DatabaseConnector()
        {
            _context = new Context();
        }

        public async Task<List<Transaction>> Get()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _context.Transactions.ToList();
                }
                catch (Exception)
                {
                    return new List<Transaction>();
                }
            });
        }
        public async Task<bool> Add(Transaction item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    item.Date = DateTime.Now;
                    _context.Transactions.Add(item);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public async Task<bool> Edit(Transaction newItem)
        {
            await Task.Run(() =>
            {
                try
                {
                    Transaction item = _context.Transactions.FirstOrDefault(el => el.Id == newItem.Id);
                    item.Date = DateTime.Now;
                    item.Ammount = newItem.Ammount;
                    item.Type = newItem.Type;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public async Task<bool> Delete(Transaction item)
        {
            await Task.Run(() =>
            {
                try
                {
                    _context.Transactions.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.Database_Context;
using Finance.Model;

namespace Finance.DatabaseConnector
{
    class DatabaseTransactionConnector : IDatabaseConnector<Transaction>
    {
        private static DatabaseTransactionConnector _databaseConnector;
        private static readonly object Locker = new object();

        private readonly Context _context;

        public static DatabaseTransactionConnector GetDatabaseConnector
        {
            get
            {
                if (_databaseConnector == null)
                {
                    lock (Locker)
                    {
                        if (_databaseConnector == null)
                            _databaseConnector = new DatabaseTransactionConnector();
                    }
                }

                return _databaseConnector;
            }
        }

        private DatabaseTransactionConnector()
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
        public async Task<Transaction> Add(Transaction item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    item.Date = DateTime.Now;
                    _context.Transactions.Add(item);
                    _context.SaveChanges();
                    return item;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
        public async Task<bool> Edit(Transaction newItem)
        {
            return await Task.Run(() =>
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
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
        public async Task<bool> Delete(Transaction item)
        {
            return await Task.Run(() =>
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

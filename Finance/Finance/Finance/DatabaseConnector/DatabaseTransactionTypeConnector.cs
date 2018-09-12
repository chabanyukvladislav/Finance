using Finance.Database_Context;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.DatabaseConnector
{
    class DatabaseTransactionTypeConnector : IDatabaseConnector<TransactionType>
    {
        private static DatabaseTransactionTypeConnector _databaseConnector;
        private static readonly object Locker = new object();

        private readonly Context _context;

        public static DatabaseTransactionTypeConnector GetDatabaseConnector
        {
            get
            {
                if (_databaseConnector == null)
                {
                    lock (Locker)
                    {
                        if (_databaseConnector == null)
                            _databaseConnector = new DatabaseTransactionTypeConnector();
                    }
                }

                return _databaseConnector;
            }
        }

        private DatabaseTransactionTypeConnector()
        {
            _context = new Context();
        }

        public async Task<List<TransactionType>> Get()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _context.TransactionTypes.ToList();
                }
                catch (Exception)
                {
                    return new List<TransactionType>();
                }
            });
        }

        public async Task<TransactionType> Add(TransactionType item)
        {
            return await Task.Run(() => { return (TransactionType)null; }); // !!!
        }
        public async Task<bool> Edit(TransactionType newItem)
        {
            return await Task.Run(() => { return false; });
        }
        public async Task<bool> Delete(TransactionType item)
        {
            return await Task.Run(() => { return false; });
        }
    }
}

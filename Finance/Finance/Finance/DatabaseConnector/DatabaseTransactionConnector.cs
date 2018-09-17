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
        public async Task<bool> Add(Transaction item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    List<Transaction> list = _context.Transactions?.OrderBy(el => el.Date)?.ToList();
                    item.Date = DateTime.Now;
                    item.Total = item.Ammount;
                    if (_context.Transactions.Count() != 0)
                    {
                        item.Total = item.Type.Value == "Income"
                            ? (Convert.ToInt32(list.Last().Total) + Convert.ToInt32(item.Ammount))
                            .ToString()
                            : (Convert.ToInt32(list.Last().Total) - Convert.ToInt32(item.Ammount))
                            .ToString();
                    }
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
            return await Task.Run(() =>
            {
                try
                {
                    Transaction preItem = _context.Transactions?.OrderBy(el => el.Date)?.LastOrDefault(el => el.Date < newItem.Date);
                    if (preItem != null)
                        newItem.Total = newItem.Type.Value == "Income"
                            ? (Convert.ToInt32(preItem.Total) + Convert.ToInt32(newItem.Ammount)).ToString()
                            : (Convert.ToInt32(preItem.Total) - Convert.ToInt32(newItem.Ammount)).ToString();
                    else
                        newItem.Total = newItem.Type.Value == "Income" ? newItem.Ammount : "-" + newItem.Ammount;
                    List<Transaction> list = _context.Transactions?.Where(el => el.Date > newItem.Date)?.OrderBy(el => el.Date)?.ToList() ?? new List<Transaction>();
                    if (list.Count > 0)
                        list[0].Total = list[0].Type.Value == "Income"
                            ? (Convert.ToInt32(newItem.Total) + Convert.ToInt32(list[0].Ammount)).ToString()
                            : (Convert.ToInt32(newItem.Total) - Convert.ToInt32(list[0].Ammount)).ToString();
                    for (int i = 1; i < list.Count; ++i)
                        list[i].Total = list[i].Type.Value == "Income"
                            ? (Convert.ToInt32(list[i - 1].Total) + Convert.ToInt32(list[i].Ammount)).ToString()
                            : (Convert.ToInt32(list[i - 1].Total) - Convert.ToInt32(list[i].Ammount)).ToString();
                    newItem.Type = _context.TransactionTypes?.FirstOrDefault(el => el.Id == newItem.Type.Id);
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
            return await Task.Run(() =>
            {
                try
                {
                    Transaction preItem = _context.Transactions?.OrderBy(el => el.Date)?.LastOrDefault(el => el.Date < item.Date);
                    List<Transaction> list = _context.Transactions?.Where(el => el.Date > item.Date)?.OrderBy(el => el.Date)?.ToList();
                    if (list.Count != 0)
                    {
                        if (preItem != null)
                            list[0].Total = list[0].Type.Value == "Income"
                                ? (Convert.ToInt32(preItem.Total) + Convert.ToInt32(list[0].Ammount)).ToString()
                                : (Convert.ToInt32(preItem.Total) - Convert.ToInt32(list[0].Ammount)).ToString();
                        else
                            list[0].Total = list[0].Type.Value == "Income" ? list[0].Ammount : "-" + list[0].Ammount;
                        for (int i = 1; i < list.Count; ++i)
                            list[i].Total = list[i].Type.Value == "Income"
                                ? (Convert.ToInt32(list[i - 1].Total) + Convert.ToInt32(list[i].Ammount)).ToString()
                                : (Convert.ToInt32(list[i - 1].Total) - Convert.ToInt32(list[i].Ammount))
                                .ToString();
                    }
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

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Finance.DatabaseConnector;
using Finance.Model;

namespace Finance.Collection
{
    class TransactionCollection : ITransactionCollection
    {

        private static TransactionCollection _transactionCollection;
        private static readonly object Locker = new object();

        private readonly IDatabaseConnector _connector;
        private ObservableCollection<Transaction> _collection;

        public static TransactionCollection GetTransactionCollection
        {
            get
            {
                if (_transactionCollection == null)
                {
                    lock (Locker)
                    {
                        if (_transactionCollection == null)
                            _transactionCollection = new TransactionCollection();
                    }
                }

                return _transactionCollection;
            }
        }

        public ObservableCollection<Transaction> Transactions
        {
            get => _collection;
            private set
            {
                _collection = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        private TransactionCollection()
        {
            _connector = DatabaseConnector.DatabaseConnector.GetDatabaseConnector;
        }

        public async void Add(Transaction item)
        {
            if(!await _connector.Add(item))
                return;
        }
        public async void Delete(Transaction item)
        {
            if (!await _connector.Delete(item))
                return;
        }
        public async void Edit(Transaction newItem)
        {
            if (!await _connector.Edit(newItem))
                return;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

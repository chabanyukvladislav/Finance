using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Finance.DatabaseConnector;
using Finance.Model;

namespace Finance.Collection
{
    class TransactionCollection : ICollection<Transaction>
    {
        private static TransactionCollection _transactionCollection;
        private static readonly object Locker = new object();

        private readonly IDatabaseConnector<Transaction> _connector;
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

        public ObservableCollection<Transaction> Collection
        {
            get => _collection;
            private set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        private TransactionCollection()
        {
            _connector = DatabaseTransactionConnector.GetDatabaseConnector;
            LoadCollection();
        }

        private async void LoadCollection()
        {
            Collection = new ObservableCollection<Transaction>((await _connector.Get()).OrderByDescending(el => el.Date));
        }

        public async void Add(Transaction item)
        {
            if (!await _connector.Add(item))
                return;
            _collection.Insert(0, item);
        }
        public async void Delete(Transaction item)
        {
            if (!await _connector.Delete(item))
                return;
            _collection.Remove(item);
        }
        public async void Edit(Transaction newItem)
        {
            if (!await _connector.Edit(newItem))
                return;
            _collection[_collection.IndexOf(newItem)] = newItem;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

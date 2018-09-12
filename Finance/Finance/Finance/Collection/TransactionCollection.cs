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
            Collection = new ObservableCollection<Transaction>(await _connector.Get());
        }

        public async void Add(Transaction item)
        {
            Transaction transaction = await _connector.Add(item);
            if (transaction == null)
                return;
            _collection.Add(transaction);
            OnPropertyChanged(nameof(Collection));
        }
        public async void Delete(Transaction item)
        {
            if (!await _connector.Delete(item))
                return;
            _collection.Remove(item);
            OnPropertyChanged(nameof(Collection));
        }
        public async void Edit(Transaction newItem)
        {
            if (!await _connector.Edit(newItem))
                return;
            _collection.Remove(_collection.FirstOrDefault(el => el.Id == newItem.Id));
            _collection.Add(newItem);
            OnPropertyChanged(nameof(Collection));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

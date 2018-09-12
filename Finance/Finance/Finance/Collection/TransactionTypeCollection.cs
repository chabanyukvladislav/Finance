using Finance.DatabaseConnector;
using Finance.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Finance.Collection
{
    class TransactionTypeCollection : ICollection<TransactionType>
    {
        private static TransactionTypeCollection _transactionTypeCollection;
        private static readonly object Locker = new object();

        private readonly IDatabaseConnector<TransactionType> _connector;
        private ObservableCollection<TransactionType> _collection;

        public static TransactionTypeCollection GetTransactionTypeCollection
        {
            get
            {
                if (_transactionTypeCollection == null)
                {
                    lock (Locker)
                    {
                        if (_transactionTypeCollection == null)
                            _transactionTypeCollection = new TransactionTypeCollection();
                    }
                }

                return _transactionTypeCollection;
            }
        }

        public ObservableCollection<TransactionType> Collection
        {
            get => _collection;
            private set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        private TransactionTypeCollection()
        {
            _connector = DatabaseTransactionTypeConnector.GetDatabaseConnector;
            LoadCollection();
        }

        private async void LoadCollection()
        {
            Collection = new ObservableCollection<TransactionType>(await _connector.Get());
        }

        public void Add(TransactionType item) { }
        public void Delete(TransactionType item) { }
        public void Edit(TransactionType newItem) { }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

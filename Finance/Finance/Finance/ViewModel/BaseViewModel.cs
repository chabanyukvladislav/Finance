using Finance.Collection;
using Finance.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Finance.ViewModel
{
    class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly ICollection<Transaction> _collection;
        protected ObservableCollection<Transaction> _transactions;
        protected Transaction _selectedItem;

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            protected set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }
        public Transaction SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public BaseViewModel()
        {
            _collection = TransactionCollection.GetTransactionCollection;
            _collection.PropertyChanged += OnCollectionPropertyChanged;
            Transactions = _collection.Collection;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Transactions = _collection.Collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Finance.Collection;
using Finance.Model;
using Finance.View;
using Xamarin.Forms;

namespace Finance.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICollection<Transaction> _collection;
        private ObservableCollection<Transaction> _transactions;
        private Transaction _selectedItem;

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            private set
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

        public ICommand Add { get; }
        public ICommand Edit { get; }
        public ICommand Delete { get; }
        public ICommand Report { get; }

        public MainViewModel()
        {
            Add = new Command(ExecuteAdd);
            Edit = new Command(ExecuteEdit);
            Delete = new Command(ExecuteDelete);
            Report = new Command(ExecuteReport);
            _collection = TransactionCollection.GetTransactionCollection;
            _collection.PropertyChanged += OnCollectionPropertyChanged;
            Transactions = _collection.Collection;
        }

        private async void ExecuteAdd()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TransactionPage());
        }
        private async void ExecuteEdit()
        {
            if(SelectedItem == null)
                return;
            await Application.Current.MainPage.Navigation.PushAsync(new TransactionPage(SelectedItem));
        }
        private void ExecuteDelete(object item)
        {
            if (SelectedItem == null)
                return;
            _collection.Delete(SelectedItem);
        }
        private async void ExecuteReport(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ReportPage());
        }

        private void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_collection.Collection))
            {
                SelectedItem = null;
                Transactions = _collection.Collection;
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Finance.Collection;
using Finance.Enum;
using Finance.Model;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Finance.ViewModel
{
    class ReportViewModel : BaseViewModel
    {
        private readonly ICollection<TransactionType> _transactionTypeCollection;
        private ObservableCollection<TransactionType> _types;
        private DateTime _from;
        private DateTime _to;
        private TransactionType _type;
        private string _sum;
        private string _lastTotal;
        private string _firstTotal;

        private TypeOfSort TypeOfSort { get; set; }

        public ObservableCollection<TransactionType> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged(nameof(Types));
            }
        }
        public TransactionType SelectedType
        {
            get => _type;
            set
            {
                _type = value;
                OnDataChanged();
                OnPropertyChanged(nameof(SelectedType));
            }
        }
        public DateTime From
        {
            get => _from;
            set
            {
                _from = value;
                OnDataChanged();
            }
        }
        public DateTime To
        {
            get => _to;
            set
            {
                _to = value;
                OnDataChanged();
            }
        }
        public string Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }
        public string FirstTotal
        {
            get => _firstTotal;
            set
            {
                _firstTotal = value;
                OnPropertyChanged(nameof(FirstTotal));
            }
        }
        public string LastTotal
        {
            get => _lastTotal;
            set
            {
                _lastTotal = value;
                OnPropertyChanged(nameof(LastTotal));
            }
        }

        public ICommand Clear { get; }
        public ICommand DateSort { get; }
        public ICommand TypeSort { get; }
        public ICommand AmmountSort { get; }

        public ReportViewModel() : base()
        {
            DateTime dt = DateTime.Now;
            _from = DateTime.Parse(dt.Month + "." + 1 + "." + dt.Year);
            _to = DateTime.Parse(dt.Month + "." + 30 + "." + dt.Year);
            Clear = new Command(ExecuteClear);
            DateSort = new Command(ExecuteDateSort);
            TypeSort = new Command(ExecuteTypeSort);
            AmmountSort = new Command(ExecuteAmmountSort);
            _transactionTypeCollection = TransactionTypeCollection.GetTransactionTypeCollection;
            _transactionTypeCollection.PropertyChanged += OnCollectionTypePropertyChanged;
            _types = _transactionTypeCollection.Collection;
            TypeOfSort = TypeOfSort.DateDescending;
            Transactions = _collection.Collection;
            if (Transactions.Count() != 0)
                OnDataChanged();
        }

        private void OnDataChanged()
        {
            Transactions = new ObservableCollection<Transaction>(_collection.Collection.Where(el => (SelectedType == null ? true : el.Type.Id == SelectedType?.Id) && (el.Date.Date >= From) && (el.Date.Date <= To)));
            int sum = 0;
            Transactions.ForEach(transaction => sum = transaction.Type.Value == "Income" ? sum + Convert.ToInt32(transaction.Ammount) : sum - Convert.ToInt32(transaction.Ammount));
            FirstTotal = "FirstTotal: " + Transactions?.OrderBy(el => el.Date)?.First()?.Total;
            LastTotal = "LastTotal: " + Transactions?.OrderBy(el => el.Date)?.Last()?.Total;
            Sum = "Sum: " + sum.ToString();
        }
        private void ExecuteClear()
        {
            SelectedType = null;
            OnDataChanged();
        }
        private void ExecuteAmmountSort()
        {
            if (TypeOfSort == TypeOfSort.Ammount)
            {
                Transactions = new ObservableCollection<Transaction>(Transactions.OrderByDescending(el => Convert.ToInt32(el.Ammount)));
                TypeOfSort = TypeOfSort.AmmountDescending;
            }
            else
            {
                Transactions = new ObservableCollection<Transaction>(Transactions.OrderBy(el => Convert.ToInt32(el.Ammount)));
                TypeOfSort = TypeOfSort.Ammount;
            }
        }
        private void ExecuteTypeSort()
        {
            if (TypeOfSort == TypeOfSort.Type)
            {
                Transactions = new ObservableCollection<Transaction>(Transactions.OrderByDescending(el => el.Type.Value));
                TypeOfSort = TypeOfSort.TypeDescending;
            }
            else
            {
                Transactions = new ObservableCollection<Transaction>(Transactions.OrderBy(el => el.Type.Value));
                TypeOfSort = TypeOfSort.Type;
            }
        }
        private void ExecuteDateSort()
        {
            if (TypeOfSort == TypeOfSort.Date)
            {
                Transactions = new ObservableCollection<Transaction>(Transactions.OrderByDescending(el => el.Date));
                TypeOfSort = TypeOfSort.DateDescending;
            }
            else
            {
                Transactions = new ObservableCollection<Transaction>(Transactions.OrderBy(el => el.Date));
                TypeOfSort = TypeOfSort.Date;
            }
        }

        private void OnCollectionTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_transactionTypeCollection.Collection))
            {
                Types = _transactionTypeCollection.Collection;
            }
        }
        protected override void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCollectionPropertyChanged(sender, e);

            OnDataChanged();
        }
    }
}

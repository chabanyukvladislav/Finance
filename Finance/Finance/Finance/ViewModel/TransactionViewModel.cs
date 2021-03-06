﻿using Finance.Collection;
using Finance.Model;
using Finance.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Finance.ViewModel
{
    class TransactionViewModel : INotifyPropertyChanged
    {
        private readonly ICollection<Transaction> _transactionCollection;
        private readonly ICollection<TransactionType> _transactionTypeCollection;
        private ObservableCollection<TransactionType> _transactionTypes;
        private ValidatableObject _transactionItem;

        public ObservableCollection<TransactionType> TransactionTypes
        {
            get => _transactionTypes;
            private set
            {
                _transactionTypes = value;
                OnPropertyChanged(nameof(TransactionTypes));
            }
        }

        public ValidatableObject TransactionItem
        {
            get => _transactionItem;
            private set
            {
                _transactionItem = value;
                OnPropertyChanged(nameof(TransactionItem));
            }
        }

        public ICommand Save { get; }

        public TransactionViewModel()
        {
            Save = new Command(ExecuteSave);
            _transactionCollection = TransactionCollection.GetTransactionCollection;
            _transactionTypeCollection = TransactionTypeCollection.GetTransactionTypeCollection;
            _transactionTypeCollection.PropertyChanged += OnCollectionPropertyChanged;
            _transactionTypes = _transactionTypeCollection.Collection;
            TransactionItem = new ValidatableObject(new Transaction(), new NumberValidator());
        }
        public TransactionViewModel(Transaction item)
        {
            Save = new Command(ExecuteSave);
            _transactionCollection = TransactionCollection.GetTransactionCollection;
            _transactionTypeCollection = TransactionTypeCollection.GetTransactionTypeCollection;
            _transactionTypeCollection.PropertyChanged += OnCollectionPropertyChanged;
            _transactionTypes = _transactionTypeCollection.Collection;
            TransactionItem = new ValidatableObject(item, new NumberValidator());
        }

        private async void ExecuteSave()
        {
            if (!TransactionItem.IsValid || TransactionItem.Value.Type == null)
                return;
            if (TransactionItem.Value.Id == 0)
                _transactionCollection.Add(TransactionItem.Value);
            else
                _transactionCollection.Edit(TransactionItem.Value);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_transactionTypeCollection.Collection))
            {
                TransactionTypes = _transactionTypeCollection.Collection;
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

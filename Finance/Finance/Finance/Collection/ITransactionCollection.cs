using System.Collections.ObjectModel;
using System.ComponentModel;
using Finance.Model;

namespace Finance.Collection
{
    interface ITransactionCollection: INotifyPropertyChanged
    {
        ObservableCollection<Transaction> Transactions { get; }

        void Add(Transaction item);
        void Edit(Transaction newItem);
        void Delete(Transaction item);
    }
}

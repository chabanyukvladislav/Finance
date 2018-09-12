using Finance.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Finance.Collection
{
    interface ICollection<T> : INotifyPropertyChanged where T : IDatabaseModel
    {
        ObservableCollection<T> Collection { get; }

        void Add(T item);
        void Edit(T newItem);
        void Delete(T item);
    }
}

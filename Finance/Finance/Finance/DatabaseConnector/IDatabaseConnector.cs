using Finance.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.DatabaseConnector
{
    interface IDatabaseConnector<T> where T : IDatabaseModel
    {
        Task<List<T>> Get();
        Task<T> Add(T item);
        Task<bool> Edit(T newItem);
        Task<bool> Delete(T item);
    }
}

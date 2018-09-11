using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Model;

namespace Finance.DatabaseConnector
{
    interface IDatabaseConnector
    {
        Task<List<Transaction>> Get();
        Task<bool> Add(Transaction item);
        Task<bool> Edit(Transaction newItem);
        Task<bool> Delete(Transaction item);
    }
}

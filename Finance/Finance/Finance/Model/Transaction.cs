using System;

namespace Finance.Model
{
    class Transaction
    {
        public int Id { get; set; }
        public int Ammount { get; set; }
        public DateTime Date { get; set; }
        public virtual Transaction_Type Type { get; set; }
    }
}

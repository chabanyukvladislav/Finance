using System;
using System.ComponentModel;

namespace Finance.Model
{
    public class Transaction : IDatabaseModel, INotifyPropertyChanged
    {
        private string _amount;

        public int Id { get; set; }
        public string Ammount
        {
            get => _amount;
            set
            {
                _amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ammount)));
            }
        }
        public DateTime Date { get; set; }
        public virtual TransactionType Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

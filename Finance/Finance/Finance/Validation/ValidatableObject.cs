using System;
using System.Collections.Generic;
using System.ComponentModel;
using Finance.Model;

namespace Finance.Validation
{
    public class ValidatableObject : INotifyPropertyChanged
    {
        private Transaction _value;

        public List<IValidationRule> Validations { get; }

        public Transaction Value
        {
            get => _value;
            set
            {
                _value = value;
                _value.PropertyChanged += OnPropertyChanged;
            }
        }

        public bool IsValid => Validations.TrueForAll(v => v.Validate(Value.Ammount));

        public ValidatableObject(params IValidationRule[] validations)
        {
            Validations = new List<IValidationRule>();
            foreach (var val in validations)
                Validations.Add(val);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

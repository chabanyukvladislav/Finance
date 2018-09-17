using System.ComponentModel;
using System.Windows.Input;
using Finance.View;
using Xamarin.Forms;

namespace Finance.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Edit { get; }
        public ICommand Delete { get; }
        public ICommand Report { get; }

        public MainViewModel() : base()
        {
            Add = new Command(ExecuteAdd);
            Edit = new Command(ExecuteEdit);
            Delete = new Command(ExecuteDelete);
            Report = new Command(ExecuteReport);
        }

        private async void ExecuteAdd()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TransactionPage());
            SelectedItem = null;
        }
        private async void ExecuteEdit()
        {
            if (SelectedItem == null)
                return;
            await Application.Current.MainPage.Navigation.PushAsync(new TransactionPage(SelectedItem));
            SelectedItem = null;
        }
        private void ExecuteDelete(object item)
        {
            if (SelectedItem == null)
                return;
            _collection.Delete(SelectedItem);
            SelectedItem = null;
        }
        private async void ExecuteReport(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ReportPage());
        }

        protected override void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCollectionPropertyChanged(sender, e);
            SelectedItem = null;
        }
    }
}

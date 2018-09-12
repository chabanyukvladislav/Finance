using Finance.Model;
using Finance.ViewModel;

namespace Finance.View
{
    public partial class TransactionPage
    {
        public TransactionPage()
        {
            InitializeComponent();
            BindingContext = new TransactionViewModel();
        }
        public TransactionPage(Transaction item)
        {
            InitializeComponent();
            BindingContext = new TransactionViewModel(item);
        }
    }
}
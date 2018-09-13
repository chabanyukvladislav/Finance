using Finance.Model;
using Xamarin.Forms;

namespace Finance.Control
{
    public partial class GridCell : Grid
    {
        public static readonly BindableProperty ItemSourceProperty;

        public Transaction ItemSource
        {
            get => (Transaction)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        static GridCell()
        {
            ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(Transaction), typeof(GridCell), new Transaction());
        }
        public GridCell()
        {
            InitializeComponent();
        }

        private void InitializeItem()
        {
            date.Text = ItemSource.Date.ToString();
            type.Text = ItemSource.Type.Value;
            amount.Text = ItemSource.Ammount.ToString();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemSourceProperty.PropertyName)
            {
                if (ItemSource == null || ItemSource.Id == 0)
                {
                    return;
                }
                InitializeItem();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BestBy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Expiring : ContentPage
    {
        UpdateItem updateItem;
        ObservableCollection<ProductGroup> Products { get; set; } = new ObservableCollection<ProductGroup>();
        public Expiring()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetCollectionViewSources();
            cv.SelectedItem = null;
        }
        private void ResetCollectionViewSources()
        {
            Products.Clear();
            var expired = DB.conn.Table<Product>().AsEnumerable().Where(p => p.LabelColor == "Red").ToList();
            var expiring = DB.conn.Table<Product>().AsEnumerable().Where(p => p.LabelColor == "DarkOrange").ToList();
            Products.Add(new ProductGroup("Expiring", expiring));
            Products.Add(new ProductGroup("Expired", expired));
            cv.ItemsSource = Products;
        }
        private async void cv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                Product product = e.CurrentSelection[0] as Product;
                if (product != null)
                {
                    updateItem = new UpdateItem(product);
                    await Navigation.PushModalAsync(updateItem, true);
                }
            }
        }
    }
}
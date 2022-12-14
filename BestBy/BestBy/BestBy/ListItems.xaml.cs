using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BestBy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListItems : ContentPage
    {
        AddItem addItem;
        UpdateItem updateItem;
        ObservableCollection<ProductGroup> Products { get;  set; } = new ObservableCollection<ProductGroup>();
        public ListItems()
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
            var groups = DB.conn.Table<Product>().Select(p => p.Category).Distinct().ToList();
            foreach (var group in groups) { 
                var groupList = DB.conn.Table<Product>().Where(p=>p.Category.Equals(group)).ToList();
                Products.Add(new ProductGroup(group, groupList));
            }
            cv.ItemsSource = Products; 
        }
        private async void NewClicked(object sender, EventArgs e)
        {
            addItem = new AddItem();
            await Navigation.PushModalAsync(addItem, true);
            ResetCollectionViewSources();
        }
        private async void cv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0) {
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
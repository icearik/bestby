using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BestBy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateItem : ContentPage
    {
        Product oldProduct;
        public UpdateItem(Product p)
        {
            InitializeComponent();
            if (p != null)
            {
                oldProduct = p;
                productName.Text = p.Name;
                expirationDate.Date = p.ExpirationDate;
                productType.SelectedItem = p.Category;
            } 
        }
        private async void UpdateClicked(object sender, EventArgs e)
        {
            int daysLeft = (expirationDate.Date - DateTime.Now).Days;
            if (productName.Text == "" || productName.Text == null)
            {
                await DisplayAlert("Fields Required", "Please make sure the fields are not empty before updating an item", "OK");
            }
            else if (daysLeft < 0)
            {
                await DisplayAlert("Invalid Date", "The item you are trying to update is already expired, consider deleting it or changing the expiration date", "OK");
            }
            else
            {
                Product prod = new Product
                {
                    Name = productName.Text,
                    ExpirationDate = expirationDate.Date,
                    Category = productType.SelectedItem.ToString()
                };
                prod.Id = oldProduct.Id;
                DB.conn.Update(prod);
                await Navigation.PopModalAsync();
            }
            
        }
        private async void CancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void DeleteClicked(object sender, EventArgs e)
        {
            Product product = oldProduct;
            if (product != null)
            {
                int v = DB.conn.Delete(product);
            }
            await Navigation.PopModalAsync();
        }
    }
}
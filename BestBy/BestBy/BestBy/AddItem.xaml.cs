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
    public partial class AddItem : ContentPage
    {
        public AddItem()
        {
            InitializeComponent();
        }
        private async void AddClicked(object sender, EventArgs e)
        {
            int daysLeft = (expirationDate.Date - DateTime.Now).Days;
            if (productType.SelectedItem == null || productName.Text == "" || productName.Text == null)  {
                await DisplayAlert("Fields Required", "Please fill all of the fields before adding an item", "OK");
            } else if (daysLeft < 0)
            {
                await DisplayAlert("Invalid Date", "The item you are trying to add is already expired", "OK");
            } else
            {
                Product prod = new Product
                {
                    Name = productName.Text.Trim(),
                    ExpirationDate = expirationDate.Date,
                    Category = productType.SelectedItem.ToString()
                };
                DB.conn.Insert(prod);
                await Navigation.PopModalAsync();
            }
        }
        private async void CancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
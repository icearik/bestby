using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace BestBy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage : ContentPage
    {
        List<string> data = new List<string>{ "item", "item", "item", "item", "item", "item", "item", "Add the drained pasta to the skillet with the creamy sauce and toss to combine. If the sauce becomes too thick, add a couple of tablespoons of the reserved pasta water and toss to combine with the sauce" };
        Recipe recipe;
        public static string endPoint = "https://api.spoonacular.com/recipes/";
        public static string APIKey = "your API KEY";
        public HttpClient client = new HttpClient();
        public RecipePage(Recipe input)
        {
            InitializeComponent();
            GetRecipe(input.Id);
        }
        private void UpdateContents() {
            title.Text = recipe.Name;
            img.Source = recipe.ImageURL;
            summary.Text = recipe.Summary;
            servings.Text = recipe.Servings.ToString();
            time.Text = $"{recipe.Time} minutes";
            credits.Text = recipe.SourceName;
            foreach (var row in recipe.Ingredients)
            {
                var item = row.GetValue("original");
                string entry = $"\u2022  {item}";
                ingredients.Children.Add(new Label { Text = entry });
            }
            if (recipe.Instructions.Count != 0)
            {
                JObject content = recipe.Instructions[0];
                JArray description = JsonConvert.DeserializeObject<JArray>(content.GetValue("steps").ToString());
                foreach (JObject step in description)
                {
                    steps.Children.Add(new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                        new Button {Text = step.GetValue("number").ToString()},
                        new Label { Text = step.GetValue("step").ToString(), VerticalOptions = LayoutOptions.Center, }
                    }
                    });
                }
            }
            else
            {
                steps.Children.Add(new Label { Text = "No Instructions provided for the recipe" });
            }
            sv.IsVisible = true;
        }

        private async void GetRecipe(int id)
        {
            string requestUri = endPoint;
            requestUri += $"{id}/information?";
            requestUri += $"&apiKey={APIKey}";
            requestUri += $"&includeNutrition=false";
            string result = null;
            try
            {
                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
                Environment.Exit(0);
            }
            recipe = JsonConvert.DeserializeObject<Recipe>(result);
            UpdateContents();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
        
            try
            {
                await Browser.OpenAsync(recipe.Source, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}

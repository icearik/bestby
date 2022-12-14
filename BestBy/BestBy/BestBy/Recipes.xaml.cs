using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Xml;
using Xamarin.Essentials;

namespace BestBy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
   
    public partial class Recipes : ContentPage
    {
        public static string endPoint = "https://api.spoonacular.com/recipes/findByIngredients";
        public static string APIKey = "Your API Key";
        public HttpClient client = new HttpClient();
        RecipePage recipePage;
        List<Recipe> mock = new List<Recipe> { 
            new Recipe {ImageURL = "", Name = "No recipes found for provided ingredients or your grocery list is empty" } };
        public Recipes()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey("Maximize"))
                Preferences.Set("Maximize", true);
            if (!Preferences.ContainsKey("ExpiringOnly"))
                Preferences.Set("ExpiringOnly", true);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetRecipees();
        }
        private async void GetRecipees()
        {
            var listOfIngredients = DB.conn.Table<Product>().Where(p => p.Category.Equals("Grocery")).Select(p => p.Name).ToList();
            if (Preferences.Get("ExpiringOnly", true)) {
                listOfIngredients = DB.conn.Table<Product>().AsEnumerable().Where(p => p.Category.Equals("Grocery") 
                    && p.LabelColor == "DarkOrange").Select(p => p.Name).ToList();
            }
            if (listOfIngredients.Count > 0) {
                string requestUri = endPoint;
                requestUri += "?&number=10";
                requestUri += $"&ranking={(Preferences.Get("Maximize", true) ? 1 : 2)}";
                requestUri += $"&apiKey={APIKey}";
                requestUri += $"&ingredients=";
                foreach (var ingredient in listOfIngredients)
                {
                    requestUri += $"{ingredient.Replace(" ", "+")},";
                }
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
                JArray recipes = JsonConvert.DeserializeObject<JArray>(result);
                List<Recipe> recipeList = new List<Recipe>();
                foreach (var row in recipes)
                {
                    Recipe recipe = JsonConvert.DeserializeObject<Recipe>(row.ToString());
                    recipeList.Add(recipe);
                }
                if (recipeList.Count > 0)
                {
                    lv.ItemsSource = recipeList;
                }
                else
                {
                    lv.ItemsSource = mock;
                }
            } else {
                lv.ItemsSource = mock;
            }
            sl.IsVisible = true;
            
        }

        private async void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Recipe recipe = lv.SelectedItem as Recipe;
            if (recipe != null)
            {
                recipePage = new RecipePage(recipe);
                await Navigation.PushAsync(recipePage, true);
            }
        }
    }
}

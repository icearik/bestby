using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace BestBy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey("Maximize"))
                Preferences.Set("Maximize", true);
            maximizeSwitch.IsToggled = Preferences.Get("Maximize", true);
            if (!Preferences.ContainsKey("ExpiringOnly"))
                Preferences.Set("ExpiringOnly", true);
            expirySwitch.IsToggled = Preferences.Get("ExpiringOnly", true);
        }
        private void MaximizeSwitchToggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("Maximize", maximizeSwitch.IsToggled);
        }


        private void ExpirySwitchToggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("ExpiringOnly", expirySwitch.IsToggled);
        }
    }
}
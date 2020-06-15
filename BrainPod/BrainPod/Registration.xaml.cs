using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
            //hide nav bar
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //pop off current page from navigations stack to go back to login
        public void LoadLogin(object sender, EventArgs e)
        {
            //pop off
            Navigation.PopAsync();
        }
        
    }
}
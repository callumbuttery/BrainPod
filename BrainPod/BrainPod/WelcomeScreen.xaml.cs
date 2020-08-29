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
    public partial class WelcomeScreen : ContentPage
    {
        public WelcomeScreen(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();

            //Welcome message with capitalised first name
            WelcomeMessage.Text = "Welcome to Brain Pod " + char.ToUpper(uFirstName[0]) + uFirstName.Substring(1) + "!";

            //set logo source
            Logo.Source = ImageSource.FromFile("Logo.png");
        }
    }
}
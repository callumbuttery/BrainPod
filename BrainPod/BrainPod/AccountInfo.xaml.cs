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
    public partial class AccountInfo : ContentPage
    {
        public AccountInfo(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();
            userEmailDisplay.Text = uEmail;
            userFirstDisplay.Text = uFirstName;
            userLastDisplay.Text = uLastName;

        }

        //signout account
        private void SignOut_Clicked(object sender, EventArgs e)
        {
            //Set new current page to the login screen
            //This way if the user presses the back arrow, the app won't display the MasterPage again
            App.Current.MainPage = new MainPage();
            
           
            
        }
    }
}
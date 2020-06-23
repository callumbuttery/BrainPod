using BrainPod.Table;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
        FirebaseAuth authU;
        public Registration()
        {
            InitializeComponent();
            //hide nav bar
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new RegisteredUsers();
            
        }

        //pop off current page from navigations stack to go back to login
        public void LoadLogin(object sender, EventArgs e)
        {
            //pop off
            Navigation.PopAsync();
        }


        public bool EmailValidator()
        {
            try
            {
                MailAddress mailAddress = new MailAddress(EmailInput.Text);
                return true;

            }
            catch
            {
                return false;
            }
        }

        async  void Button_Clicked(object sender, EventArgs e)
        {
            var result = await firebaseClient
               .Child("RegisteredUsers")
               .PostAsync(new RegisteredUsers() { Email = EmailInput.Text, Password = PasswordInput.Text, FirstName = FirstNameInput.Text, LastName = SecondNameInput.Text });

            if (result.Object != null)
            {
                await DisplayAlert("Registration", "Successfully registered", "Close");

            }
            else
            {
                await DisplayAlert("Registration", "Failed registration, please try again", "Close");

            }
        }
    }
}
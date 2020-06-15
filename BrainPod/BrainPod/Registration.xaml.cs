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

        public void validation(Object sender, EventArgs e)
        {
            try
            {
                if (FirstNameInput.Text == null || SecondNameInput.Text == null || EmailInput.Text == null || PasswordInput.Text == null)
                {
                    DisplayAlert("Blank input", "Please ensure a value has been entered into each box", "Retry");
                    return;
                }
                else
                {
                    //call module to check email
                    bool emailVal = EmailValidator();

                    //display if email is false etc
                }
            }
            catch
            {
                DisplayAlert("Registration failure", "Sorry the mouse fell off the wheel", "Retry");
                return;
            }
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
        
    }
}
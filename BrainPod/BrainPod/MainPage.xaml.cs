using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BrainPod
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Logo.Source = ImageSource.FromFile("Logo.png");
        }

        public void signInClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            bool emailVerified;

            try
            {
                if (emailEntry.Text != null)
                {
                    //emailVerified set to value returned from EmailValidation module
                    emailVerified = EmailValidation(email);

                    if (emailVerified != true)
                    {
                        DisplayAlert("Email error", "Invalid email format", "Retry");
                        return;
                    }
                    else
                    {
                        //DisplayAlert("Email correct", "Correct email format", "Okay");
                        //Email passes all checks, move onto checking password 
                        PasswordVerification();


                    }

                }
                else
                {
                    DisplayAlert("Email error", "Email is blank or null", "Retry");
                    return;
                }


            }
            catch
            {
                DisplayAlert("Login Failure", "The mouse fell off its wheel", "Retry");
                return;
            }

           }
            

        //Used to check if email entered is a valid email format
        public bool EmailValidation(string email)
        {
            try
            {
                //New email address object, pass in email provided by user
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //verify password
        public void PasswordVerification()
        {
            //if password entry box isn't blank enter if
            if (passwordEntry.Text != null)
            {
                string password = passwordEntry.Text;

                //checks for password length
                if (password.Length < 6)
                {
                    DisplayAlert("Password error", "Password must be bigger than 6 characters", "Retry");
                    passwordEntry.Text = string.Empty;
                    return;
                }
                else
                {
                    //proceed to check users account exists
                }
            }
        }

    }
}

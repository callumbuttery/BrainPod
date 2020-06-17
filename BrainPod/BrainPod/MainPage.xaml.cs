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
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //used to load logo from files
            //Logo.source is created in xaml file
            Logo.Source = ImageSource.FromFile("Logo.png");
            //hide nav bar
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void signInClicked(object sender, EventArgs e)
        {
            //receive email from input box
            string email = emailEntry.Text;
            //used to verify if the user has entered a valid email address
            bool emailVerified;

            try
            {
                //enter if entry box to enter email isn't blank
                if (emailEntry.Text != null)
                {
                    //emailVerified set to value returned from EmailValidation module
                    emailVerified = EmailValidation(email);

                    if (emailVerified != true)
                    {
                        //display error
                        DisplayAlert("Email error", "Invalid email format", "Retry");
                        //return user to re-enter
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
                    //alert user to email error
                    DisplayAlert("Email error", "Email is blank or null", "Retry");
                    return;
                }


            }
            catch
            {
                //report code failure to user
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
                    //password validation error
                    DisplayAlert("Password error", "Password must be bigger than 6 characters", "Retry");
                    //set password entry box to empty
                    passwordEntry.Text = string.Empty;
                    return;
                }
                else
                {
                    //proceed to check users account exists on backend
                    Navigation.PushAsync(new Slider());
                }
            }
        }

        //load registration form
        private void LoadReg(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Registration());
        }
    }
}

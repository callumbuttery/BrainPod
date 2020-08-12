using BrainPod.Table;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BrainPod
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
        public MainPage()
        {

            InitializeComponent();
            //Logo.source is created in xaml file
            Logo.Source = ImageSource.FromFile("Logo.png");
            //hide nav bar
            NavigationPage.SetHasNavigationBar(this, false);

            var currentNetworkAccess = Connectivity.NetworkAccess;

            if(currentNetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Network Failure", "Please connect to a network to use all features of this app", "Close");
                return; 
            }

           
        }

        //called by button click
        async void Button_Clicked(object sender, EventArgs e)
        {
            //checks if username and password are registered to a user
            var validuser = await CheckUserExists();

            
            

            //if returned user isn't null then load tabbedmaster page
            //need to add functionality to load users saved journal logs
            if(validuser != null)
            {
                string userEmail = validuser.Email;
                string userFirstName = validuser.FirstName;
                string userLastName = validuser.LastName;
                Guid userID = validuser.UserID;

                //await Navigation.PushAsync(new TabbedMaster(userEmail, userFirstName, userLastName, userID));
                //User is logged in, load MasterPage
                Application.Current.MainPage = new MasterPage(userEmail, userFirstName, userLastName, userID);
            }
        }



        private async Task<RegisteredUsers> CheckUserExists()
        {
            //get values entered by yser
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            //show loading wheel on screen
            LoadingWheel.IsRunning = true;

            //fetch account based on information provided by user
            var getUser = (await firebaseClient
                .Child("RegisteredUsers")
                .OnceAsync<RegisteredUsers>()).Where(a => a.Object.Email == email).Where(b => b.Object.Password == password).FirstOrDefault();

            //No longer need to await a process, hide loading wheel from screen
            LoadingWheel.IsRunning = false;

            //if getUser equals null then the user has entered incorrect infomation
            if (getUser == null)
            {
                await DisplayAlert("Login Failed", "Please check your email and password", "Return");
                return null;
            }
            //valid user, return data, successful login
            else
            {
                var Content = getUser.Object as RegisteredUsers;
                return Content;
                
                
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
           
        }

        //load registration form
        private void LoadReg(object sender, EventArgs e)
        {
            //pop off tabbedmaster page from nav stacks
            Application.Current.MainPage = new NavigationPage(new Registration());
        }

 
    }
}

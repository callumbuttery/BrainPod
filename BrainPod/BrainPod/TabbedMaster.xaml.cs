using BrainPod.Table;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMaster : TabbedPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        List<UserLogs> foundLogs = new List<UserLogs>();
        List<string> testing = new List<string>();

        public TabbedMaster(string userEmail, string userFirstName, string userLastName, Guid userID)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LogBtn.IsEnabled = false;



            //read data passed from login screen
            userEmailDisplay.Text = userEmail;
            userFirstDisplay.Text = userFirstName;
            userLastDisplay.Text = userLastName;
            userIDDisplay.Text = userID.ToString();

            WelcomeMessage.Text = ("Welcome back " + userFirstName + "!");

            
            //recieve instances of userlogs to display
            RecieveInstances(userID);
           

            var noInstances = "No logs were found";
            string[] testing = new string[] { "this is a test" };


            //ListOfLogs.ItemsSource = testing;
            

        }

        bool SliderChanged = false;

        //recieve instances of user logs from firebase
        async void RecieveInstances(Guid userID)
        {

            /*getInstance will hold a list 
             * of log instances with the matching userID
             * */
            foundLogs = (await firebaseClient
            .Child("UserLogs")
            .OnceAsync<UserLogs>()).Where(a => a.Object.UserID == userID).Select(item => new UserLogs
            {
                UserID = item.Object.UserID,
                logData = item.Object.logData,
                sliderValue = item.Object.sliderValue,
                logTime = item.Object.logTime

            }).ToList();

            //set listview source to the list returned from backend
            listOfLogs.ItemsSource = foundLogs;


            /*Need to work out a way to display the newest log first
             * rather than the oldest log first 
             */

            /*Need to work out how to add new logs to screen without closing
             * and reopening the app
             */
        }


        //handles a change in slider value
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SliderValue.Text = DayRatingSlider.Value.ToString();
            SliderChanged = true;
            
            
        }

       
        //handles editor text entry is complete
        private void JournalEntry_Completed(object sender, EventArgs e)
        {
            string journalEntry = JournalEntry.Text;
        }

        //checks if user has used slider
        private void JournalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if slider used then enable button to submit log as log has been entered and slider has been used
            if(SliderChanged == true && JournalEntry.Text != null)
            {
                LogBtn.IsEnabled = true;


            }

        }

        //store user data
        async void LogBtn_Clicked(object sender, EventArgs e)
        {
            string sliderVal = DayRatingSlider.Value.ToString();
            string journalVal = JournalEntry.Text;
            DateTime dt = DateTime.Now;
            string convertedDateTime = dt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            try
            {
                Guid idAsGuid = new Guid(userIDDisplay.Text);
                //stores response from firebase
                var result = await firebaseClient
                   .Child("UserLogs")
                   //posts new log to databse
                   .PostAsync(new UserLogs() { UserID = idAsGuid, logData = journalVal, sliderValue = sliderVal, logTime = convertedDateTime });

                //reset values
                DayRatingSlider.Value = 0;
                JournalEntry.Text = "";
                
            }
            catch
            {
                await DisplayAlert("Failure", "The mouse couldn't add your journal log, please try again", "Retry");
            }
            

        }


        //Signed out, so return user to login screen
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}
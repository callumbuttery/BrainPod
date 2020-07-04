using BrainPod.Table;
using Firebase.Database;
using Firebase.Database.Query;
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
    public partial class TabbedMaster : TabbedPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
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
            
        }

        bool SliderChanged = false;

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

            try
            {
                //stores response from firebase
                var result = await firebaseClient
                   .Child("UserLogs")
                   //posts new log to databse
                   .PostAsync(new UserLogs() { UserID = userIDDisplay.Text, logData = journalVal, sliderValue = sliderVal });

                DayRatingSlider.Value = 0;
                JournalEntry.Text = "";
                
            }
            catch
            {
                await DisplayAlert("Failure", "The mouse couldn't add your journal log, please try again", "Retry");
            }
            

        }
    }
}
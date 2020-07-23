using BrainPod.Table;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Journal : ContentPage
    {

        //globals
        bool SliderChanged = false;
        string uEmail;
        string uFirstName;
        string uLastName;
        Guid uID;

        public Journal(string userEmail, string userFirstName, string userLastName, Guid userID)
        {
            InitializeComponent();
            uEmail = userEmail;
            uFirstName = userFirstName;
            uLastName = userLastName;
            uID = userID;
            
        }

        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");


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
            if (SliderChanged == true && JournalEntry.Text != null)
            {
                LogBtn.IsEnabled = true;
            }

        }

        //store user data
        async void LogBtn_Clicked(object sender, EventArgs e)
        {
            //get slidervalue
            string sliderVal = DayRatingSlider.Value.ToString();
            //get journal value
            string journalVal = JournalEntry.Text;
            //get current date
            DateTime dt = DateTime.Now;
            //append datetime to correct format
            string convertedDateTime = dt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            try
            {
                //get users accountID
                //Guid idAsGuid = new Guid(uID);
                //stores response from firebase
                var result = await firebaseClient
                   .Child("UserLogs")
                   //posts new log to databse
                   .PostAsync(new UserLogs() { UserID = uID, logData = journalVal, sliderValue = sliderVal, logTime = convertedDateTime });

                //reset values
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
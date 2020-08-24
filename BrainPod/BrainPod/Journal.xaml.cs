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
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");


        //globals
        bool SliderChanged = false;
        string faceClicked;
        bool faceClickedBool = false;
        string uEmail;
        string uFirstName;
        string uLastName;
        Guid uID;

        public Journal(string userEmail, string userFirstName, string userLastName, Guid userID)
        {
            InitializeComponent();

            sadFace.BackgroundColor = Color.White;
            middleFace.BackgroundColor = Color.White;
            happyFace.BackgroundColor = Color.White;

            uEmail = userEmail;
            uFirstName = userFirstName;
            uLastName = userLastName;
            uID = userID;

            LogBtn.IsEnabled = false;

            DatePicker.MinimumDate = DateTime.Now;
            DatePicker.MaximumDate = DateTime.Now;
            DatePicker.Format = "dd-MM-yy";

        }


        //handles a change in slider value
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //get slidervalue
            double sliderVal = DayRatingSlider.Value;
            //convert to int and remove decimals
            int convertedSliderVal = Convert.ToInt32(sliderVal);


            SliderValue.Text = convertedSliderVal.ToString();
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

        private void sadFaceClicked(object sender, EventArgs e)
        {
            faceClicked = "bad";
            faceClickedBool = true;
            sadFace.BackgroundColor = Color.FromHex("#FF2C26");
            middleFace.BackgroundColor = Color.White;
            happyFace.BackgroundColor = Color.White;
        }

        private void middleFaceClicked(object sender, EventArgs e)
        {
            faceClicked = "middle";
            faceClickedBool = true;
            sadFace.BackgroundColor = Color.White;
            middleFace.BackgroundColor = Color.FromHex("#F4FF54");
            happyFace.BackgroundColor = Color.White;
        }

        private void happyFaceClicked(object sender, EventArgs e)
        {
            faceClicked = "good";
            faceClickedBool = true;
            sadFace.BackgroundColor = Color.White;
            middleFace.BackgroundColor = Color.White;
            happyFace.BackgroundColor = Color.FromHex("#4AFF51");
        }

        //store user data
        async void LogBtn_Clicked(object sender, EventArgs e)
        {
            //get slidervalue
            double sliderVal = DayRatingSlider.Value;

            //Convert to int to remove decimals
            int convertedSliderVal = Convert.ToInt32(sliderVal);

            //convert to string for storage
            string sConvertedSliderVal = convertedSliderVal.ToString();


            //get journal value
            string journalVal = JournalEntry.Text;
            //get current date
            DateTime dt = DateTime.Now;
            //append datetime to correct format
            string convertedDateTime = dt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);


            /*Prevent users from entering
             *slider value and journal to enable log button 
             before deleting journal*/

            if (journalVal != "" && SliderChanged == true && faceClickedBool == true)
            {
                try
                {
                    //get users accountID
                    //Guid idAsGuid = new Guid(uID);
                    //stores response from firebase
                    var result = await firebaseClient
                       .Child("UserLogs")
                       //posts new log to databse
                       .PostAsync(new UserLogs() { UserID = uID, logData = journalVal, sliderValue = sConvertedSliderVal, logTime = convertedDateTime, mood = faceClicked });

                    //reset values
                    DayRatingSlider.Value = 0;
                    JournalEntry.Text = "";
                    sadFace.BackgroundColor = Color.White;
                    middleFace.BackgroundColor = Color.White;
                    happyFace.BackgroundColor = Color.White;

                }
                catch
                {
                    await DisplayAlert("Failure", "The mouse couldn't add your journal log, please try again", "Retry");
                }
            }
            else
            {
                await DisplayAlert("Failure", "Please make sure your journal entry isn't blank, please try again", "Retry");
                return;
            }


        }
    }
}
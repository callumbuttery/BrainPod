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
    public partial class Events : ContentPage
    {
        //Globals
        int AnxietySliderValue = 0;
        int WorrySliderValue = 0;
        Guid UserID;

        bool anxSliderMoved = false;
        bool WorryingSliderMoved = false;

        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        public Events(Guid uID)
        {
            InitializeComponent();
            UserID = uID;

            CalendarLogo.Source = "https://cdn4.iconfinder.com/data/icons/small-n-flat/24/calendar-512.png";
            DatePicker.MinimumDate = DateTime.Now;
            DatePicker.MaximumDate = DateTime.Now.AddMonths(4);
            DatePicker.Format = "dd-MM-yy";
            AnxietyLevelDisplay.Text = "Anxiety level about event: ";
            WorryingThroughtLabel.Text = "Level of worry about event: ";
        }

        private void AnxietyLevelSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //get original values
            double originalValue = AnxietyLevelSlider.Value;
            //need to convert to int to remove decimal values
            AnxietySliderValue = Convert.ToInt32(originalValue);
            AnxietyLevelDisplay.Text = "Anxiety level about event: " + AnxietySliderValue;
            anxSliderMoved = true;

        }

        private void WorryingThoughtSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //get original values
            double originalValue = WorryingThoughtSlider.Value;
            //need to convert to int to remove decimal values
            WorrySliderValue = Convert.ToInt32(originalValue);
            WorryingThroughtLabel.Text = "Level of worry about event: " + WorrySliderValue;
            WorryingSliderMoved = true;
        }

        //handle event click
        private void Button_Clicked(object sender, EventArgs e)
        {
            if(EventTitleBox.Text != "" && ReasonBox.Text != "" && anxSliderMoved == true && WorryingSliderMoved == true)
            {
                //accept submission as everything as been provided
                DateTime eventDT = DatePicker.Date;

                string eventTitle = EventTitleBox.Text;
                string reasons = ReasonBox.Text;

                int anxSliderLevel = AnxietySliderValue;
                int worrySliderLevel = WorrySliderValue;

                pushNewEvent(UserID, eventDT, eventTitle, reasons, anxSliderLevel, worrySliderLevel);

            }
            else
            {
                DisplayAlert("Failure", "Please ensure all information has been provided", "Return");
                return;
            }
        }

        public async void pushNewEvent(Guid UserID, DateTime eventDT, string EventTitle, string reasons, int anxSliderLevel, int worrySliderLevel)
        {
            //append datetime to correct format
            string convertedDateTime = eventDT.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


            try
            {
                //stores response from firebase
                await firebaseClient
                   .Child("newEvent")
                   //posts new event to databse
                   //generate new event ID
                   .PostAsync(new newEvent() { userID = UserID, EventID = Guid.NewGuid() ,eventTitle = EventTitle, reason = reasons, eventDate = convertedDateTime, anxietyLevel = anxSliderLevel, worryLevel = worrySliderLevel });

                //Display successful log
                await DisplayAlert("Success!", "New event added successfully","Close");

            }
            catch
            {
                await DisplayAlert("Failed", "Failed to add new event", "retry");
                return;
            }

            await Navigation.PushAsync(new Events(UserID));
        }
    }
}
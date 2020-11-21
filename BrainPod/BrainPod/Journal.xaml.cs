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

        //holds users list of daily activites 
        List<string> listofactivites = new List<string>();

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
            faceClicked = "Bad";
            faceClickedBool = true;
            sadFace.BackgroundColor = Color.FromHex("#FF2C26");
            middleFace.BackgroundColor = Color.White;
            happyFace.BackgroundColor = Color.White;
        }

        private void middleFaceClicked(object sender, EventArgs e)
        {
            faceClicked = "Average";
            faceClickedBool = true;
            sadFace.BackgroundColor = Color.White;
            middleFace.BackgroundColor = Color.FromHex("#F4FF54");
            happyFace.BackgroundColor = Color.White;
        }

        private void happyFaceClicked(object sender, EventArgs e)
        {
            faceClicked = "Good";
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
            string convertedDate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string convertedTime = dt.ToString("HH:mm tt");


           
            //journal must not be blank, slider value must have changed, face clicked must be true and list of activities must not be empty or null
            //in order to enable the submit log button to prevent blank values being logged
            if (journalVal != "" && SliderChanged == true && faceClickedBool == true && listofactivites.Count != 0 && listofactivites != null)
            {
                try
                {
                    //convert list of activities to a string for storage
                    string activitiesString = String.Join(",", listofactivites.ToArray());

                    //get users accountID
                    //Guid idAsGuid = new Guid(uID);
                    //stores response from firebase
                    var result = await firebaseClient
                       .Child("UserLogs")
                       //posts new log to databse
                       .PostAsync(new UserLogs() { UserID = uID, logData = journalVal, sliderValue = sConvertedSliderVal, logTime = convertedTime, logDate = convertedDate, mood = faceClicked, activities = activitiesString});

                    //reset values
                    DayRatingSlider.Value = 0;
                    JournalEntry.Text = "";
                    sadFace.BackgroundColor = Color.White;
                    middleFace.BackgroundColor = Color.White;
                    happyFace.BackgroundColor = Color.White;

                    //reset button colours
                    WorkButton.BackgroundColor = Color.White; 
                    StudyButton.BackgroundColor = Color.White;
                    ExerciseButton.BackgroundColor = Color.White;
                    StretchButton.BackgroundColor = Color.White;
                    SocialiseButton.BackgroundColor = Color.White;
                    GameButton.BackgroundColor = Color.White;
                    NapButton.BackgroundColor = Color.White;
                    MovieButton.BackgroundColor = Color.White;
                    AlcoholButton.BackgroundColor = Color.White;
                    EatOutButton.BackgroundColor = Color.White;
                    DateButton.BackgroundColor = Color.White;
                    ShoppingButton.BackgroundColor = Color.White;
                    ReadButton.BackgroundColor = Color.White;
                    CleanButton.BackgroundColor = Color.White;
                    EatHealthyButton.BackgroundColor = Color.White;
                    EarlySleepButton.BackgroundColor = Color.White;



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

        //add the users activities for that day to a list
        private void addActivityToList(object sender, EventArgs e)
        {
            //read button ID to work out which activity the user has clicked
            var buttonID = sender as ImageButton;
            string activityType = buttonID.ClassId;

            //stores the colour returned from validateActivity
            string returnValue = "Color.White";

            
            //reads ID of whatever button has been clicked
            switch(buttonID.ClassId)
            {
                case "WorkButton":
                    returnValue = validateActivity(" Worked");
                    break;
                case "StudyButton":
                    returnValue = validateActivity(" Studied");
                    break;
                case "ExerciseButton":
                    returnValue = validateActivity(" Exercised");
                    break;
                case "StretchButton":
                    returnValue = validateActivity(" Stretched");
                    break;
                case "SocialiseButton":
                    returnValue = validateActivity(" Socialised");
                    break;
                case "GameButton":
                    returnValue = validateActivity(" Gamed");
                    break;
                case "NapButton":
                    returnValue = validateActivity(" Napped");
                    break;
                case "MovieButton":
                    returnValue = validateActivity(" Watched Movie");
                    break;
                case "AlcoholButton":
                    returnValue = validateActivity(" Drank Alcohol");
                    break;
                case "EatOutButton":
                    returnValue = validateActivity(" Ate out");
                    break;
                case "ShoppingButton":
                    returnValue = validateActivity(" Went shopping");
                    break;
                case "DateButton":
                    returnValue = validateActivity(" Went on a date");
                    break;
                case "ReadButton":
                    returnValue = validateActivity(" Read");
                    break;
                case "CleanButton":
                    returnValue = validateActivity(" Cleaned");
                    break;
                case "EatHealthyButton":
                    returnValue = validateActivity(" Ate healthy");
                    break;
                case "EarlySleepButton":
                    returnValue = validateActivity(" Slept early");
                    break;

            }

            if(returnValue == "Color.White")
            {
                buttonID.BackgroundColor = Color.White;
            }
            else
            {
                buttonID.BackgroundColor = Color.FromHex("#4AFF51");
            }
        }

        //used to check if the activity has already been added to the list to prevent duplicates
        private string validateActivity(string activityType)
        {

            //if activity not in list then add to list
            if (!listofactivites.Contains(activityType))
            {
                //add to list
                listofactivites.Add(activityType);
                //return value to update button background to show its been clicked and added
                return "#4AFF51";

                
            }
            else
            {
                //remove from list as button has been pressed to deselect it
                listofactivites.Remove(activityType);
                //return value to update update button background to show its been clicked and added
                return "Color.White";
            }
        }
    }
}
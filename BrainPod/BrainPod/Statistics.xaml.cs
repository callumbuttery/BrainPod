using BrainPod.Table;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Statistics : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        public Statistics(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();           

            //Logo.source is created in xaml file
            //Logo.Source = ImageSource.FromFile("Logo.png");
            //checks if username and password are registered to a user
            getLogScores(uID);

        }

        public async void getLogScores(Guid uID)
        {

            //fetch all logs connected to the users ID (uID)
            var getValues = (await firebaseClient
                .Child("UserLogs")
                .OnceAsync<UserLogs>()).Where(a => a.Object.UserID == uID).Select(item => new UserLogs
            {
                sliderValue = item.Object.sliderValue,

            }).ToList();


            //get the number of logs to work out average
            int countLogs = getValues.Count;
            //total slider score for average
            int total = 0;
            //used to store int after converted from double
            int convertedItem = 0;
            //used to store average
            int average = 0;

            //loop for each item in object
            foreach (var item in getValues)
            {
                //parse data as exception thrown if a double makes it through
                if (int.TryParse(item.sliderValue, out convertedItem))
                {
                    //add up total
                    total = total + convertedItem;
                }

                //work out average
                average = total / countLogs;

            }

            noOfEntries.Text = getValues.Count.ToString();
            
            overallHappinessInt.Text = average.ToString() + "/10";

            if (average < 3)
            {
                HappinessLogo.Source = ImageSource.FromFile("sadSmile.png");
            }
            if (average > 3 & average < 6)
            {
                HappinessLogo.Source = ImageSource.FromFile("middleSmile.png");
            }
            if (average > 6 & average < 11)
            {
                HappinessLogo.Source = ImageSource.FromFile("happySmile.png");
            }

            //fetch account based on information provided by user
            var getUser = (await firebaseClient
                .Child("UserLogs")
                .OnceAsync<UserLogs>()).Where(a => a.Object.UserID == uID).LastOrDefault();

            //if getUser equals null then the user has no log history
            if (getUser == null)
            {
                logDate.Text = "No recent logs found";

            }
            //valid logs found, return data, successful login
            else
            {
                //receive activities
                var activites = getUser.Object.activities;
                List<string> activitesList = new List<string>();

                //split string on comma value(s)
                activitesList = activites.Split(',').ToList<string>();

                //change background colour of activity icon to show it was selected by the user in the original journal
                if (activitesList.Contains("WorkButton"))
                {
                    WorkButton.BackgroundColor = Color.FromHex("#FF99FF");
                    WorkFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("StudyButton"))
                {
                    StudyButton.BackgroundColor = Color.FromHex("#FF99FF");
                    StudyFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("ExerciseButton"))
                {
                    ExerciseButton.BackgroundColor = Color.FromHex("#FF99FF");
                    ExerciseFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("StretchButton"))
                {
                    StretchButton.BackgroundColor = Color.FromHex("#FF99FF");
                    StretchFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("SocialiseButton"))
                {
                    SocialiseButton.BackgroundColor = Color.FromHex("#FF99FF");
                    SocialFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("GameButton"))
                {
                    GameButton.BackgroundColor = Color.FromHex("#FF99FF");
                    GameFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("NapButton"))
                {
                    NapButton.BackgroundColor = Color.FromHex("#FF99FF");
                    NapFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("MovieButton"))
                {
                    MovieButton.BackgroundColor = Color.FromHex("#FF99FF");
                    MovieFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("AlcoholButton"))
                {
                    AlcoholButton.BackgroundColor = Color.FromHex("#FF99FF");
                    AlcoholFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("EatOutButton"))
                {
                    EatOutButton.BackgroundColor = Color.FromHex("#FF99FF");
                    EatOutFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("ShoppingButton"))
                {
                    ShoppingButton.BackgroundColor = Color.FromHex("#FF99FF");
                    ShoppingFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("DateButton"))
                {
                    DateButton.BackgroundColor = Color.FromHex("#FF99FF");
                    DateFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("ReadButton"))
                {
                    ReadButton.BackgroundColor = Color.FromHex("#FF99FF");
                    ReadFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("CleanButton"))
                {
                    CleanButton.BackgroundColor = Color.FromHex("#FF99FF");
                    CleanFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("EatHealthyButton"))
                {
                    EatHealthyButton.BackgroundColor = Color.FromHex("#FF99FF");
                    EatHealthyFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains("EarlySleepButton"))
                {
                    EarlySleepButton.BackgroundColor = Color.FromHex("#FF99FF");
                    EarlySleepFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }

                //find space in datetime (logtime)
                int spaceIndex = getUser.Object.logTime.IndexOf(" ");
                //remove all chars after found space
                string trimmedDateTime = getUser.Object.logTime.Substring(0, spaceIndex);
                //display
                logDate.Text = trimmedDateTime;
                logData.Text = '"' + getUser.Object.logData + '"';
                happinessRating.Text = getUser.Object.sliderValue;


                switch(getUser.Object.mood)
                {
                    case "bad":
                        Emoji.Source = ImageSource.FromFile("sadSmile.png");
                        break;
                    case "middle":
                        Emoji.Source = ImageSource.FromFile("middleSmile.png");
                        break;
                    case "good":
                        Emoji.Source = ImageSource.FromFile("happySmile.png");
                        break;

                }
                

            }
            

        }

        public void addActivityToList(object sender, EventArgs  e)
        {

        }
    }
}
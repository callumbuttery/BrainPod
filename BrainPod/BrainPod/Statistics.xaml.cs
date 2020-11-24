using BrainPod.Table;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Statistics : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //global user ID
        Guid id;

        public Statistics(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            Device.SetFlags(new string[] { "Expander_Experimental" });
            InitializeComponent();

            id = uID;

            sadLogo.Source = ImageSource.FromFile("sadSmiley.png");
            sadLogo.Opacity = 0.25;
            middleLogo.Source = ImageSource.FromFile("middleSmile.png");
            middleLogo.Opacity = 0.25;
            HappinessLogo.Source = ImageSource.FromFile("happySmile.png");
            HappinessLogo.Opacity = 0.25;

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
                sadLogo.Opacity = 1;
            }
            if (average > 3 & average < 6)
            {
                middleLogo.Opacity = 1;
            }
            if (average > 6 & average < 11)
            {
                HappinessLogo.Opacity = 1;
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
                //display most recent log date
                logDate.Text = getUser.Object.logDate;
                logTime.Text = getUser.Object.logTime;


                //receive activities
                var activites = getUser.Object.activities;
                List<string> activitesList = new List<string>();

                //split string on comma value(s)
                activitesList = activites.Split(',').ToList<string>();

                //change background colour of activity icon to show it was selected by the user in the original journal
                if (activitesList.Contains(" Worked"))
                {
                    WorkButton.BackgroundColor = Color.FromHex("#FF99FF");
                    WorkFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Studied"))
                {
                    StudyButton.BackgroundColor = Color.FromHex("#FF99FF");
                    StudyFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Exercised"))
                {
                    ExerciseButton.BackgroundColor = Color.FromHex("#FF99FF");
                    ExerciseFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Stretched"))
                {
                    StretchButton.BackgroundColor = Color.FromHex("#FF99FF");
                    StretchFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Socialised"))
                {
                    SocialiseButton.BackgroundColor = Color.FromHex("#FF99FF");
                    SocialFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Gamed"))
                {
                    GameButton.BackgroundColor = Color.FromHex("#FF99FF");
                    GameFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Napped"))
                {
                    NapButton.BackgroundColor = Color.FromHex("#FF99FF");
                    NapFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Watched Movie"))
                {
                    MovieButton.BackgroundColor = Color.FromHex("#FF99FF");
                    MovieFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Drank Alcohol"))
                {
                    AlcoholButton.BackgroundColor = Color.FromHex("#FF99FF");
                    AlcoholFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Ate out"))
                {
                    EatOutButton.BackgroundColor = Color.FromHex("#FF99FF");
                    EatOutFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Went shopping"))
                {
                    ShoppingButton.BackgroundColor = Color.FromHex("#FF99FF");
                    ShoppingFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Went on a date"))
                {
                    DateButton.BackgroundColor = Color.FromHex("#FF99FF");
                    DateFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Read"))
                {
                    ReadButton.BackgroundColor = Color.FromHex("#FF99FF");
                    ReadFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Cleaned"))
                {
                    CleanButton.BackgroundColor = Color.FromHex("#FF99FF");
                    CleanFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Ate healthy"))
                {
                    EatHealthyButton.BackgroundColor = Color.FromHex("#FF99FF");
                    EatHealthyFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }
                if (activitesList.Contains(" Slept early"))
                {
                    EarlySleepButton.BackgroundColor = Color.FromHex("#FF99FF");
                    EarlySleepFrame.BackgroundColor = Color.FromHex("#FF99FF");
                }

                //find space in datetime (logtime)
                int spaceIndex = getUser.Object.logTime.IndexOf(" ");
                //remove all chars after found space
                string trimmedDateTime = getUser.Object.logTime.Substring(0, spaceIndex);
                //display
                //logDate.Text = trimmedDateTime;
                logData.Text = '"' + getUser.Object.logData + '"';
                happinessRating.Text = getUser.Object.sliderValue;


    
                

            }
            

        }

        //searches for user records
        async void searchFunction(object sender, EventArgs e)
        {
            //get button id so we know what the user wants to search for e.g. happiness or activities
            var button = (Button)sender;
            var buttonClassID = button.ClassId;

            //list to store logs returned from backend
            List<UserLogs> foundLogs = new List<UserLogs>();

            try
            {
                //search based on happiness rating
                if (buttonClassID == "happinessButton")
                {
                    //check user has entered a value
                    if (!string.IsNullOrEmpty(happinessEntry.Text))
                    {
                        int value = Int32.Parse(happinessEntry.Text);
                        if (value < 0 || value > 10)
                        {
                            return;
                        }

                        foundLogs.Clear();
                        foundLogs = (await firebaseClient
                            .Child("UserLogs")
                            .OnceAsync<UserLogs>()).Where(a => a.Object.UserID == id).Where(b => b.Object.sliderValue == happinessEntry.Text).Select(item => new UserLogs
                            {
                                UserID = item.Object.UserID,
                                logData = item.Object.logData,
                                sliderValue = item.Object.sliderValue,
                                logTime = item.Object.logTime,
                                logDate = item.Object.logDate,
                                activities = item.Object.activities,
                                mood = item.Object.mood,



                            }).ToList();

                        //check a log has been found
                        if(foundLogs.Count == 0 || foundLogs == null)
                        {
                            await DisplayAlert("No data found", "No data found based on the terms you have searched for", "Retry");

                        }
                        else
                        {
                           
                            //give list view source
                            happinessFilterList.ItemsSource = foundLogs;
                        }
                        
                    }
                    else
                    {
                        await DisplayAlert("Oh no", "Please enter a number between 1 and 10", "Retry");
                        return;
                    }
                }

                //handled activity button click
                if(buttonClassID == "activityButton")
                {
                    //get selected activity
                    string selectedActivity = activityPicker.SelectedItem.ToString();
                    //insure not null
                    if (!string.IsNullOrEmpty(selectedActivity))
                    {
                        //clear any previous logs
                        foundLogs.Clear();
                        foundLogs = (await firebaseClient
                            .Child("UserLogs")
                            .OnceAsync<UserLogs>()).Where(a => a.Object.UserID == id).Where(b => b.Object.activities.Contains(selectedActivity)).Select(item => new UserLogs
                            {
                                UserID = item.Object.UserID,
                                logData = item.Object.logData,
                                sliderValue = item.Object.sliderValue,
                                logTime = item.Object.logTime,
                                logDate = item.Object.logDate,
                                activities = item.Object.activities,
                                mood = item.Object.mood,



                            }).ToList();

                        //check a log has been found
                        if (foundLogs.Count == 0 || foundLogs == null)
                        {
                            await DisplayAlert("No data found", "No data found based on the terms you have searched for", "Retry");

                        }
                        else
                        {

                            //give list view source
                            activityFilterList.ItemsSource = foundLogs;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Oh no", "Please select a task from the menu", "Retry");
                        return;
                    }
                    

                }
            }
            catch (Exception excep)
            {
                await DisplayAlert("Uh oh", excep.ToString(), "retry");
                return;
            }
        }


        public void addActivityToList(object sender, EventArgs  e)
        {

        }
    }
}
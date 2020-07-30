using Android.Text.Method;
using BrainPod.Table;
using Firebase.Database;
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
    public partial class Statistics : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        public Statistics(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();           

            //Logo.source is created in xaml file
            Logo.Source = ImageSource.FromFile("Logo.png");
            //Welcome message with capitalised first name
            WelcomeMessage.Text = "Welcome to Brain Pod " + char.ToUpper(uFirstName[0]) + uFirstName.Substring(1) + "!";
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

            overallHappiness.Text = "Your average happiness rating:";
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
                logDate.Text = getUser.Object.logTime;
                logData.Text = getUser.Object.logData;
                happinessRating.Text = getUser.Object.sliderValue;


            }
            

        }
    }
}
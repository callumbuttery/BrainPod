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
    public partial class JournalLogs : ContentPage
    {
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //list to store logs returned from backend
        List<UserLogs> foundLogs = new List<UserLogs>();

        public JournalLogs(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();

            Guid userID = uID;
            jLogo.Source = ImageSource.FromFile("JournalLogo.png");

            WelcomeMessage.Text = ("Journal Entries");

            //recieve instances of userlogs to display
            RecieveInstances(userID);
        }

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
                logTime = item.Object.logTime,
                logDate = item.Object.logDate,
                activities = item.Object.activities,
                mood = item.Object.mood,

               

            }).ToList();


            //code for box which counts journal entries etc

            //get the number of logs to work out average
            int countLogs = foundLogs.Count;
            //total slider score for average
            int total = 0;
            //used to store int after converted from double
            int convertedItem = 0;
            //used to store average
            int average = 0;


            //loop for each item in object
            foreach (var item in foundLogs)
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

            noOfEntries.Text = foundLogs.Count.ToString();

            overallHappinessInt.Text = average.ToString() + "/10";

            //get most recent log times
            var logObject = foundLogs.LastOrDefault();

            mostRecentLogDate.Text = logObject.logDate;
            mostRecentLogTime.Text = logObject.logTime;
            

            

            var activities = logObject.activities;
            List<string> activitiesList = new List<string>();

            activitiesList = activities.Split(',').ToList<string>();

            //order list so most recently added log is positioned first
            var orderedLogs = foundLogs.OrderByDescending(x => x.logTime).ThenBy(y => y.logDate).ToList();
            //set listview source to the list returned from backend 
            listOfLogs.ItemsSource = orderedLogs;

        }
    }
}
using BrainPod.Table;
using Firebase.Auth;
using Firebase.Database;
using Java.Util.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountInfo : ContentPage
    {
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
                //list to store logs returned from backend
        List<UserLogs> foundLogs = new List<UserLogs>();

        public AccountInfo(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();

            getLogs(uID);
            userEmailDisplay.Text = "Email: " + uEmail;
            userFirstDisplay.Text = "First name: " + uFirstName;
            userLastDisplay.Text = "Last name: " + uLastName;

            

        }

        async void getLogs(Guid userID)
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

            int numberOfLogs = foundLogs.Count;

            //display number of recorded logs on screen
            logCounter.Text = "Number of journal entries: " + numberOfLogs;


            //Get all instances of recorded PHQ9 tests
            var getResults = (await firebaseClient
                .Child("phq9Results")
                .OnceAsync<phq9Results>()).Where(a => a.Object.UserID == userID).Select(item => new phq9Results
                {
                    UserID = item.Object.UserID,
                    overallResult = item.Object.overallResult,
                    submissionDate = item.Object.submissionDate

                }).ToList();

            //count number of instances
            int numberOfTests = getResults.Count;

            //display number of recorded tests on screen
            phq9Counter.Text = "Number of PHQ9 entries: " + numberOfTests;

        }

        //signout account
        private void SignOut_Clicked(object sender, EventArgs e)
        {
            //Set new current page to the login screen
            //This way if the user presses the back arrow, the app won't display the MasterPage again
            App.Current.MainPage = new MainPage();
            
           
            
        }

    }
}
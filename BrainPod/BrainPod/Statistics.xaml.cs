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
        List<LogScores> scores = new List<LogScores>();

        public Statistics(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();

            //Logo.source is created in xaml file
            Logo.Source = ImageSource.FromFile("Logo.png");
            //Welcome message with capitalised first name
            WelcomeMessage.Text = "Welcome to Brain Pod " + char.ToUpper(uFirstName[0]) + uFirstName.Substring(1) + "!";
            //checks if username and password are registered to a user
            getLastLogInstance(uID);

        }

        public async void getLastLogInstance(Guid uID)
        {

            //fetch last log user had information provided by user
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
                var Content = getUser.Object as UserLogs;
                logDate.Text = Content.logTime;
                logData.Text = Content.logData;
                happinessRating.Text = Content.sliderValue;


            }


        }
    }
}
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

            WelcomeMessage.Text = ("Welcome back " + uFirstName + "!");

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
                logTime = item.Object.logTime

            }).ToList();

            //order list so most recently added log is positioned first
            var orderedLogs = foundLogs.OrderByDescending(x => x.logTime).ToList();

            //set listview source to the list returned from backend 
            listOfLogs.ItemsSource = orderedLogs;



        }
    }
}
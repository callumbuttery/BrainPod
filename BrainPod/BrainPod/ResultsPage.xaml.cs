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
    public partial class ResultsPage : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
        Guid idToPass = new Guid();
        public ResultsPage(int score, Guid GlobalUserID)
        {
            InitializeComponent();
            idToPass = GlobalUserID;
            DisplayResults(score);
        }

         

        public void DisplayResults(int score)
        {
            //display score to screen
            ResultsLabel.Text = score.ToString();

            //Depending on score, display certain information
            if(score >=0 && score <=4)
            {
                WarningLabel.Text = "Your test score suggests minimal to no symptoms or signs of depression";
                SuggestionsLabel.Text = "Please use this test every few weeks to monitor your health";
            }
            if(score >=5 && score <=9)
            {
                WarningLabel.Text = "Your test score suggests you are experiencing mild symptoms related to depression which are unlikely to be causing disruption in your life";
                SuggestionsLabel.Text = "Please use this test every few weeks to monitor your health. Consider using our Journal feature when required";
            }
            if(score >=10 && score <=14)
            {
                WarningLabel.Text = "Your test score suggests you are experiencing moderate symptoms related to depression which could be causing disruption in your life";
                SuggestionsLabel.Text = "Please consider seeking a trained professionals opinion as to what steps you should take from here";
            }
            if(score>=15 && score <=19)
            {
                WarningLabel.Text = "Your test score suggests you are experiencing moderately severe symtpoms related to depression which could be casuing disruption in your life";
                SuggestionsLabel.Text = "Please consider seeking help from a trained professional as this will be benefical to your situation";
            }
            if(score>=20 && score <=27)
            {
                WarningLabel.Text = "Your test score suggests you are experiencing severe syptoms related to depression";
                SuggestionsLabel.Text = "Consider seeking help from a trained medical professional to help improve your situation, if you're experiencing thoughts of suicide or self harm please consider contacting samaritans";

            }
        }

        public void Button_Clicked(object sender, EventArgs e)
        {
            returnUser();
        }

        public async void returnUser()
        {
            try
            {
                //need to get users details again in order to recall TabbedMaster page
                //fetch account based on information provided by user
                var getUser = (await firebaseClient
                    .Child("RegisteredUsers")
                    .OnceAsync<RegisteredUsers>()).Where(a => a.Object.UserID == idToPass).FirstOrDefault();

                var Content = getUser.Object as RegisteredUsers;

                //read content data returned from firebase
                var userEmail = Content.Email;
                var userFirstName = Content.FirstName;
                var userLastName = Content.LastName;
                Guid userID = Content.UserID;

                await Navigation.PushAsync(new MasterPage(userEmail, userFirstName, userLastName, userID));
            }
            catch
            {
                return;
            }
        }
    }
}
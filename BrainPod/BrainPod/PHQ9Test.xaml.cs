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
    public partial class PHQ9Test : ContentPage
    {
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //holds all questions
        string[] questions = { "How often do you have little interest or pleasure in doing things?",
        "How often do you feeling down, depressed, or hopeless?",
        "How often do you have trouble falling or staying asleep, or sleeping too much?",
        "How often are you feeling tired or having little energy?",
        "How often do you have a poor appetite or overeating?",
        "How often do you feeling bad about yourself — or that you are a failure or have let yourself or your family down?",
        "How often do you have Trouble concentrating on things, such as reading the newspaper or watching television?",
        "How often do are you moving or speaking so slowly that other people could have noticed? Or so fidgety or restless that you have been moving a lot more than usual?",
        "How often do you have thoughts that you would be better off dead, or thoughts of hurting yourself in some way?"};

        //holds all question scores
        int[] questionScores = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Guid GlobalUserID;
        int counter = 0;
        int score = 0;

        public PHQ9Test(Guid idAsGuid)
        {
            InitializeComponent();
            DisplayQuestion.Text = questions[counter];
            GlobalUserID = idAsGuid;
        }

        private void NotAtAllClicked(object sender, EventArgs e)
        {
            if (counter < 8)
            {
                questionScores[counter] = 0;
                counter++;
                DisplayQuestion.Text = questions[counter];
            }
            else
            {
                TestEnding();
            }

        }

        private void SeveralDays(object sender, EventArgs e)
        {
            if (counter < 8)
            {
                questionScores[counter] = 1;
                counter++;
                DisplayQuestion.Text = questions[counter];
            }
            else
            {
                TestEnding();
            }
        }

        private void MoreThanHalf(object sender, EventArgs e)
        {
            if (counter < 8)
            {
                questionScores[counter] = 2;
                counter++;
                DisplayQuestion.Text = questions[counter];
            }
            else
            {
                TestEnding();
            }
        }

        private void NearlyEveryday(object sender, EventArgs e)
        {
            if (counter < 8)
            {
                questionScores[counter] = 3;
                counter++;
                DisplayQuestion.Text = questions[counter];
            }
            else
            {
                DisplayQuestion.Text = "";
                TestEnding();
            }
        }

        //handles what to do once the test has ended
        public void TestEnding()
        {
            //total score
            foreach(var item in questionScores)
            {
                score += item;
            }

            //disable buttons for testing
            Button1.IsEnabled = false;
            Button2.IsEnabled = false;
            Button3.IsEnabled = false;
            Button4.IsEnabled = false;

            submitTestResults();
        }

        public async void submitTestResults()
        {
            //Generate new date time and convert to appropriate format
            //get current date
            DateTime dt = DateTime.Now;
            //append datetime to correct format
            string convertedDateTime = dt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            string convertedDateTimeWithoutTime = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string year = dt.ToString("yyyy", CultureInfo.InvariantCulture);

            

            try
            {
                //stores response from firebase
                await firebaseClient
                   .Child("phq9Results")
                   //posts new log to databse
                   .PostAsync(new phq9Results() { UserID = GlobalUserID, submissionDate = convertedDateTime, overallResult = score, submissionDateWithoutTime = convertedDateTimeWithoutTime,year = year});

                //open results window
                
            }
            catch
            {
                await DisplayAlert("Failed", "Failed to store results", "retry");
            }

            await Navigation.PushAsync(new ResultsPage(score, GlobalUserID));
        }




    }
}
using BrainPod.Table;
using Firebase.Database;
using Microcharts;
using SkiaSharp;
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
            //jLogo.Source = ImageSource.FromFile("JournalLogo.png");

            WelcomeMessage.Text = ("Journal Entries");

            //recieve instances of userlogs to display
            RecieveInstances(userID);
        }

        //recieve instances of user logs from firebase
        async void RecieveInstances(Guid userID)
        {


            int countLogs = 0;
            /*getInstance will hold a list 
             * of log instances with the matching userID
             * */
            try
            {
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



                //counters
                float goodcounter = 0;
                float averagecounter = 0;
                float badcounter = 0;


                //workout number of happy logs for graph info
                foreach (var item in foundLogs)
                {

                    if(item.mood == "Good")
                    {
                        goodcounter++;
                    }
                    if(item.mood == "Average")
                    {
                        averagecounter++;
                    }
                    if(item.mood == "Bad")
                    {
                        badcounter++;
                    }

                    
                }


                ChartEntry[] entries = new[]
                    {
                        //initialise entries
                        new ChartEntry(goodcounter)
                        {
                            Label = "Happy",
                            ValueLabel = goodcounter.ToString(),
                            Color = SKColor.Parse("#7CFC00"),
                            TextColor = SKColor.Parse("#7C40A9"),
                            
                            
                        },

                        new ChartEntry(averagecounter)
                        {
                            Label = "Average",
                            ValueLabel = averagecounter.ToString(),
                            Color = SKColor.Parse("#ffff00"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry(badcounter)
                        {
                            Label = "Poorly",
                            ValueLabel = badcounter.ToString(),
                            Color = SKColor.Parse("#ff0000"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        }
                    };


                //call to initialise chart
                AssignValues(entries);

                //get the number of logs to work out average
                countLogs = foundLogs.Count;


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

                if (logObject != null)
                {
                    mostRecentLogDate.Text = logObject.logDate;
                    mostRecentLogTime.Text = logObject.logTime;





                    var activities = logObject.activities;
                    List<string> activitiesList = new List<string>();

                    activitiesList = activities.Split(',').ToList<string>();

                    //order list so most recently added log is positioned first
                    var orderedLogs = foundLogs.OrderByDescending(x => x.logDate).ThenBy(y => y.logTime).ToList();
                    //set listview source to the list returned from backend 
                    listOfLogs.ItemsSource = orderedLogs;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Uh oh", ex.ToString(), "Retry");
            }

        }


        public void AssignValues(ChartEntry[] entries)
        {
            //initalise chart
            ChartViewBar.Chart = new BarChart
            {
                Entries = entries,
                LabelTextSize = 50,
                IsAnimated = true,
                
                ValueLabelOrientation = Orientation.Horizontal,
                
            };
        }

    }
}
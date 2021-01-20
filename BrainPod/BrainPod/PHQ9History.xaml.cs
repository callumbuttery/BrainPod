using BrainPod.Table;
using Firebase.Database;
using Microcharts;
using SkiaSharp;
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
    public partial class PHQ9History : ContentPage
    {
        //firebase connection
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //store values to be displayed on graph
        public static string[] dtLabel = { "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        public static int[] scoreLabel = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static float test1 = 1;
        public static float test2 = 2;

        //list
        List<phq9Results> getResults = new List<phq9Results>();

        public PHQ9History(Guid idAsGuid)
        {
            InitializeComponent();
            //initalise chart
            ChartViewBar.Chart = new LineChart
            {
                
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 40,
                IsAnimated = true,
                LineSize = 20,
                LabelOrientation = Orientation.Horizontal
            };

            DisplayContent(idAsGuid);
        }

        public async void DisplayContent(Guid idAsGuid)
        {
            try
            {
                /*getInstance will hold a list 
             * of result instances with the matching userID
             * */
                getResults = (await firebaseClient
                .Child("phq9Results")
                .OnceAsync<phq9Results>()).Where(a => a.Object.UserID == idAsGuid).Select(item => new phq9Results
                {
                    UserID = item.Object.UserID,
                    overallResult = item.Object.overallResult,
                    submissionDate = item.Object.submissionDate

                }).ToList();

                //order list so most recently added result is positioned first
                var orderedResults = getResults.OrderByDescending(x => x.submissionDate).ToList();

                listOfResults.ItemsSource = orderedResults;


                //user must log 10 entries to display 
                if (orderedResults.Count >= 9)
                {

                    //counter for loop below
                    int count = 0;
                    while (count < 9)
                    {
                        foreach (var item in getResults)
                        {
                            if (count < 9)
                            {
                                //get data
                                dtLabel[count] = item.submissionDate;
                                scoreLabel[count] = item.overallResult;
                                //increment counter
                                count++;
                            }
                        }
                    }

                    ChartEntry[] entries = new[]
                    {

                        new ChartEntry (test1)
                        {
                            Label = dtLabel[0],
                            ValueLabel = scoreLabel[0].ToString(),
                            Color = SKColor.Parse("#ff4422")
                        },

                        new ChartEntry (test2)
                        {
                            Label = "text",
                            ValueLabel = "2",
                            Color = SKColor.Parse("#b455b6")
                        }
                    };

                    //call method to display findings
                    AssignValues(entries);

                }


            }
            catch(Exception e)
            {
                return;
            }


        }

        public void AssignValues(ChartEntry[] entries)
        {
           

            //initalise chart
            ChartViewBar.Chart = new LineChart
            {
                Entries = entries,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 40,
                IsAnimated = true,
                LineSize = 20,
                LabelOrientation = Orientation.Horizontal
            };
        }
    
       
    }
}
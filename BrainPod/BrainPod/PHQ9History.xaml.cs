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
                LabelOrientation = Orientation.Vertical
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
                        foreach (var item in orderedResults)
                        {
                            if (count < 9)
                            {
                                //get data and remove time part of date time
                                string cleanup = item.submissionDate.Substring(0, 10);
                                //remove the 20 in 2021 for display removes
                                dtLabel[count] = cleanup.Remove(6, 2);

                                //store score
                                scoreLabel[count] = item.overallResult;
                                //increment counter
                                count++;
                            }
                        }
                    }

                    ChartEntry[] entries = new[]
                    {

                        new ChartEntry (scoreLabel[0])
                        {
                            Label = dtLabel[0],
                            ValueLabel = scoreLabel[0].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[1])
                        {
                            Label = dtLabel[1],
                            ValueLabel = scoreLabel[1].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[2])
                        {
                            Label = dtLabel[2],
                            ValueLabel = scoreLabel[2].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[3])
                        {
                            Label = dtLabel[3],
                            ValueLabel = scoreLabel[3].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),

                        },

                        new ChartEntry (scoreLabel[4])
                        {
                            Label = dtLabel[4],
                            ValueLabel = scoreLabel[4].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[5])
                        {
                            Label = dtLabel[5],
                            ValueLabel = scoreLabel[5].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[6])
                        {
                            Label = dtLabel[6],
                            ValueLabel = scoreLabel[6].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[7])
                        {
                            Label = dtLabel[7],
                            ValueLabel = scoreLabel[7].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },

                        new ChartEntry (scoreLabel[8])
                        {
                            Label = dtLabel[8],
                            ValueLabel = scoreLabel[8].ToString(),
                            Color = SKColor.Parse("#7C40A9"),
                            TextColor = SKColor.Parse("#7C40A9"),
                        },
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
                LabelTextSize = 50,
                IsAnimated = true,
                LineMode = LineMode.Spline,
                PointMode = PointMode.Circle,
                PointSize = 20,
                LabelOrientation = Orientation.Vertical,
               
            };
        }
    
       
    }
}
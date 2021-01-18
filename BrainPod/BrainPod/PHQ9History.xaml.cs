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
    public partial class PHQ9History : ContentPage
    {
        //firebase connection
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //list
        List<phq9Results> getResults = new List<phq9Results>();
        
        public PHQ9History(Guid idAsGuid)
        {
            InitializeComponent();
            //initalise chart
            ChartViewBar.Chart = new LineChart { Entries = entries, ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 40, IsAnimated = true, LineSize = 20, LabelOrientation = Orientation.Horizontal};

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

                

            }
            catch
            {
                return;
            }
           

        }

        private ChartEntry[] entries = new[]
           {

                new ChartEntry (1)
                {
                    Label = "ios",
                    ValueLabel = "1",
                    Color = SKColor.Parse("#ff4422")
                },

                new ChartEntry (2)
                {
                    Label = "text",
                    ValueLabel = "2",
                    Color = SKColor.Parse("#b455b6")
                }
            };
    }
}
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
    public partial class EventsHistory : ContentPage
    {
        //global
        Guid uID;

        //firebase connection
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        public EventsHistory(Guid id)
        {
            InitializeComponent();

            uID = id;

            //get all events
            getEventHistory(id);
        }

        async void getEventHistory(Guid id)
        {
            try
            {
                //list to store events returned from backend
                List<newEvent> eventlog = new List<newEvent>();

                //get history from user ID
                //searches for criteria where logs have been updated
                eventlog.Clear();
                eventlog = (await firebaseClient
                    .Child("newEvent")
                    .OnceAsync<newEvent>()).Where(a => a.Object.userID == id).Select(item => new newEvent
                    {
                        userID = item.Object.userID,
                        EventID = item.Object.EventID,
                        eventTitle = item.Object.eventTitle,
                        eventDate = item.Object.eventDate,
                        reason = item.Object.reason,
                        anxietyLevel = item.Object.anxietyLevel,
                        worryLevel = item.Object.worryLevel,
                        factsfeeling = item.Object.factsfeeling,
                        scenarioEvaluation = item.Object.scenarioEvaluation,
                        positiveNotes = item.Object.positiveNotes,


                    }).ToList();

                //set list source
                listOfEvents.ItemsSource = eventlog;
            }
            catch(Exception ex)
            {
                //display error
                await DisplayAlert("Uh oh", "Please show this to support: \n\n" + ex.ToString(), "Okay");
            }
        }
    }
}
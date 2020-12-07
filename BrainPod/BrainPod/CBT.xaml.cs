using BrainPod.Table;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CBT : ContentPage
    {
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //globals
        Guid id;

        //for events
        string eventFactFeelings = "";
        string positives = "";
        string scenario = "";

        string newFeelings = "";
        public CBT(Guid uID)
        {
            Device.SetFlags(new string[] { "Expander_Experimental" });
            InitializeComponent();

            //store user ID, required to identify database data
            id = uID;

            //get CBT log history
            cbtHistory(id);
            //get events history
            eventsCall(id);
        }

        //handles button click to store entered data 
        async void submitData(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var buttonClassID = button.ClassId;

            //set to true if a text box is blank
            bool nullCode = false;

            //validate
            if(thoughtBox.Text == null || thoughtBox.Text == "")
            {
                thoughtBox.Placeholder = "Please enter some text";
                nullCode = true;
            }

            //validate
            if (evidenceBox.Text == null || evidenceBox.Text == "")
            {
                evidenceBox.Placeholder = "Please enter some text";
                nullCode = true;
            }

            //validate
            if (factsfeelingsBox.Text == null || factsfeelingsBox.Text == "")
            {
                factsfeelingsBox.Placeholder = "Please enter some text";
                nullCode = true;
            }

            //validate
            if (likelyScenarioBox.Text == null || likelyScenarioBox.Text == "")
            {
                likelyScenarioBox.Placeholder = "Please enter some text";
                nullCode = true;
            }

            //validate
            if(positiveBox.Text == null || positiveBox.Text == "")
            {
                positiveBox.Placeholder = "Please enter some text";
                nullCode = true;
            }

            if(nullCode == true)
            {
                return;
            }
            else
            {
                //generate new uniqueID 
                Guid postID = Guid.NewGuid();
                //post data to database
                var data = await firebaseClient
                    .Child("cbtEntry")
                    .PostAsync(new cbtEntry() { uID = id, cbtEntryID = postID, thoughts = thoughtBox.Text, evidence = evidenceBox.Text, scenarioEvaluation = likelyScenarioBox.Text, factsfeeling = factsfeelingsBox.Text, positiveNotes = positiveBox.Text, updatedNotes = "false" });

                //rotate button to show complete
                await button.RotateTo(360, 0500);
                button.Rotation = 0;

                //clear entries
                thoughtBox.Text = "";
                evidenceBox.Text = "";
                likelyScenarioBox.Text = "";
                factsfeelingsBox.Text = "";
                positiveBox.Text = "";


            }
        }

        async void cbtHistory(Guid ID)
        {
            try
            {
                //list to store logs returned from backend
                List<cbtEntry> cbtLogs = new List<cbtEntry>();

                //second list to store logs returned from backend with different criteria 
                List<cbtEntry> completetcbtLogs = new List<cbtEntry>();

                //searches for criteria where logs have been updated
                completetcbtLogs.Clear();
                completetcbtLogs = (await firebaseClient
                    .Child("cbtEntry")
                    .OnceAsync<cbtEntry>()).Where(a => a.Object.uID == ID).Where(b => b.Object.updatedNotes != "false").Select(item => new cbtEntry
                    {
                        uID = item.Object.uID,
                        cbtEntryID = item.Object.cbtEntryID,
                        thoughts = item.Object.thoughts,
                        evidence = item.Object.evidence,
                        factsfeeling = item.Object.factsfeeling,
                        scenarioEvaluation = item.Object.scenarioEvaluation,
                        positiveNotes = item.Object.positiveNotes,
                        updatedNotes = item.Object.updatedNotes

                    }).ToList();

                cbtLogs.Clear();
                cbtLogs = (await firebaseClient
                    .Child("cbtEntry")
                    .OnceAsync<cbtEntry>()).Where(a => a.Object.uID == ID).Where(b => b.Object.updatedNotes == "false").Select(item => new cbtEntry
                    {
                        uID = item.Object.uID,
                        cbtEntryID = item.Object.cbtEntryID,
                        thoughts = item.Object.thoughts,
                        evidence = item.Object.evidence,
                        factsfeeling = item.Object.factsfeeling,
                        scenarioEvaluation = item.Object.scenarioEvaluation,
                        positiveNotes = item.Object.positiveNotes,

                    }).ToList();

                //render list
                cbtHistoryList.ItemsSource = cbtLogs;
                completecbtHistoryList.ItemsSource = completetcbtLogs;
            }
            catch(Exception e)
            {
                //log error
                await DisplayAlert("Error", "Send this to support \n\n" + e.ToString(), "Retry");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            cbtHistory(id);
        }

        private async void submit(object sender, EventArgs e)
        {
            //submit new data to database
            var button = (Button)sender;

            //buttonID is the same as the cas ID in each rendered cbt entry
            var cbtID = button.ClassId;

            //convert from string to guid
            Guid guid = new Guid(cbtID);

            //get and store details of current case before deleting
            try
            {
                var getPost = (await firebaseClient
                    .Child("cbtEntry")
                    .OnceAsync<cbtEntry>()).Where(a => a.Object.cbtEntryID == guid).FirstOrDefault();

                //store returned details
                var cbtEntryID = getPost.Object.cbtEntryID;
                var uID = getPost.Object.uID;
                var evidence = getPost.Object.evidence;
                var factsFeeling = getPost.Object.factsfeeling;
                var postiveNotes = getPost.Object.positiveNotes;
                var thoughts = getPost.Object.thoughts;
                var scenario = getPost.Object.scenarioEvaluation;

                //delete old entry
                await firebaseClient.Child("cbtEntry").Child(getPost.Key).DeleteAsync();

                //add upated record
                var postRecord = await firebaseClient
                    .Child("cbtEntry")
                    .PostAsync(new cbtEntry() { uID = uID, cbtEntryID = cbtEntryID, thoughts = thoughts, evidence = evidence, factsfeeling = factsFeeling, scenarioEvaluation = scenario, positiveNotes = postiveNotes, updatedNotes = newFeelings });

                //rotate button to show complete
                await button.RotateTo(360, 0300);
                button.Rotation = 0;

                //refresh list 
                cbtHistory(uID);
            }
            catch(Exception ex)
            {
                
                //log error
                await DisplayAlert("Error", "Send this to support \n\n" + ex.ToString(), "Retry");

                return;

            }
        }

        private void updateDetailsEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (Editor)sender;
            //update global variable
            newFeelings = editor.Text;
        }

        //used to get list of events user has logged in events tab
        private void eventsRefresh(object sender, EventArgs e)
        {
            //call method
            eventsCall(id);
        }

        async void eventsCall(Guid id)
        {
            //create new list
            List<newEvent> eventHistory = new List<newEvent>();

            //get list of events
            eventHistory.Clear();
            eventHistory = (await firebaseClient
                .Child("newEvent")
                .OnceAsync<newEvent>()).Where(a => a.Object.userID == id).Select(item => new newEvent
                {
                    userID = item.Object.userID,
                    EventID = item.Object.EventID,
                    eventDate = item.Object.eventDate,
                    eventTitle = item.Object.eventTitle,
                    reason = item.Object.reason,
                    worryLevel = item.Object.worryLevel,
                    anxietyLevel = item.Object.anxietyLevel

                }).ToList();


            //assign list source 
            eventsHistoryList.ItemsSource = eventHistory;
        }

        //validate inputs
        private void submitNewEventData(object sender, EventArgs e)
        {
            if(eventFactFeelings == null)
            {
                DisplayAlert("Uh oh", "Please enter text in all boxes", "Retry");
                return;
            }
            if (positives == null)
            {
                DisplayAlert("Uh oh", "Please enter text in all boxes", "Retry");
                return;
            }
            if(scenario == null)
            {
                DisplayAlert("Uh oh", "Please enter text in all boxes", "Retry");
                return;
            }

            //push new data to database
            try
            {
                //access button classID to get EventID
                Button bt = (Button)sender;

                string eID = bt.ClassId;

                //module call
                postUpdatedEventDetails(eID);
            }
            catch (Exception ex)
            {
                DisplayAlert("Uh oh", "Please show this to support: \n\n" + ex.ToString() , "Retry");
                return;
            }
        }

        async void postUpdatedEventDetails(string eventID)
        {
            try
            {
                //convert ID string to guid
                Guid g = new Guid(eventID);

                //retrieve
                var getPost = (await firebaseClient
                       .Child("newEvent")
                       .OnceAsync<newEvent>()).Where(a => a.Object.EventID == g).FirstOrDefault();

                //store data
                var EventID = getPost.Object.EventID;
                var title = getPost.Object.eventTitle;
                var reason = getPost.Object.reason;
                var Worry = getPost.Object.worryLevel;
                var anxiety = getPost.Object.anxietyLevel;
                var date = getPost.Object.eventDate;

                //delete old entry
                await firebaseClient.Child("newEvent").Child(getPost.Key).DeleteAsync();

                //add upated record
                var postRecord = await firebaseClient
                    .Child("newEvent")
                    .PostAsync(new newEvent() { userID = id, EventID = g, reason = reason, worryLevel = Worry, anxietyLevel = anxiety, eventDate = date, eventTitle = title, factsfeeling = eventFactFeelings, positiveNotes = positives, scenarioEvaluation = scenario});

                //refresh
                eventsCall(id);


            }

            catch (Exception ex)
            {
                await DisplayAlert("Uh oh", "Please show this to support: \n\n" + ex.ToString(), "Retry");
                return;
            }
        }

        private void factsfeelingsChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (Editor)sender;
            //update global variable
            eventFactFeelings = editor.Text;
        }

        private void posChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (Editor)sender;
            //update global variable
            positives = editor.Text;
        }

        private void scenarioChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (Editor)sender;
            //update global variable
            scenario = editor.Text;
        }
    }
}
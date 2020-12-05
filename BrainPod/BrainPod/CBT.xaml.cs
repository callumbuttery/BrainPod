﻿using BrainPod.Table;
using Firebase.Database;
using Firebase.Database.Query;
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
    public partial class CBT : ContentPage
    {
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        //globals
        Guid id;

        string newFeelings = "";
        public CBT(Guid uID)
        {
            Device.SetFlags(new string[] { "Expander_Experimental" });
            InitializeComponent();

            //store user ID, required to identify database data
            id = uID;

            //get CBT log history
            cbtHistory(id);
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
            }
            catch(Exception e)
            {
                //log error
                await DisplayAlert("Error","Send this to support" + e.ToString(), "Retry");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            cbtHistory(id);
        }

        private void submit(object sender, EventArgs e)
        {

        }

        private void updateDetailsEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (Editor)sender;
            newFeelings = editor.Text;
        }
    }
}
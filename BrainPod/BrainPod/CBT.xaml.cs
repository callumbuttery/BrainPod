using BrainPod.Table;
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
        public CBT(Guid uID)
        {
            InitializeComponent();

            //store user ID, required to identify database data
            id = uID;
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
                    .PostAsync(new cbtEntry() { uID = id, cbtEntryID = postID, thoughts = thoughtBox.Text, evidence = evidenceBox.Text, scenarioEvaluation = likelyScenarioBox.Text, factsfeeling = factsfeelingsBox.Text, positiveNotes = positiveBox.Text });

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
    }
}
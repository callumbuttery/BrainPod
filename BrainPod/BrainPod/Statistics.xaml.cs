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
    public partial class Statistics : ContentPage
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
        List<LogScores> scores = new List<LogScores>();

        public Statistics(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();

           // var validuser = getStats(uEmail, uFirstName, uLastName, uID);
        }

        //public async Task<LogScores> getStats(string uEmail, string uFirstName, string uLastName, Guid uID)
        //{
            
        //}
    }
}
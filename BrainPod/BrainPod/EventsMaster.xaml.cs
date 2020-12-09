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
    public partial class EventsMaster : ContentPage
    {
        //Store uID
        Guid userID;
        public EventsMaster(Guid uID)
        {

            InitializeComponent();
            userID = uID;
            CalendarLogo.Source = "https://cdn4.iconfinder.com/data/icons/small-n-flat/24/calendar-512.png";
        }

      

        //open new events page to add new event
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Events(userID));
        }

        //open event history tab
        private void ViewHistory(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EventsHistory(userID));
        }

        //opens CBT tab
        private void loadCBT(object sender, EventArgs e)
        {
            //push CBT on top of nav stack
            Navigation.PushAsync( new CBT(userID));
        }
    }
}
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
    public partial class MasterPage : MasterDetailPage
    {

        //globals
        string uEmail;
        string uFirstName;
        string uLastName;
        Guid uID;

        public MasterPage(string userEmail, string userFirstName, string userLastName, Guid userID)
        {
            InitializeComponent();
            //hide menu bar
            IsPresented = false;

            NavigationPage.SetHasNavigationBar(this, false);


            uEmail = userEmail;
            uFirstName = userFirstName;
            uLastName = userLastName;
            uID = userID;

            //Detail = new NavigationPage(new JournalLogs(uEmail, uFirstName, uLastName, uID));
            Detail = new NavigationPage(new Statistics(uEmail, uFirstName, uLastName, uID));
        }

       //open journal logs
        private void Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new JournalLogs(uEmail, uFirstName, uLastName, uID))
            {
                BarBackgroundColor = Color.LightSkyBlue
            };
            //hide menu bar
            IsPresented = false;
        }

        //open journal logger
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Journal(uEmail,uFirstName,uLastName,uID))
            {
                BarBackgroundColor = Color.LightSeaGreen
               
            };
            //hide menu bar
            IsPresented = false;
        }

        //Display PHQ9 master
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PHQ9Master(uEmail, uFirstName, uLastName, uID))
            {
                BarBackgroundColor = Color.LightSalmon
            };
            //hide menu bar
            IsPresented = false;
        }

        //Display account info page
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AccountInfo(uEmail, uFirstName, uLastName, uID))
            {
                BarBackgroundColor = Color.LightGray
            };
            //hide menu bar
            IsPresented = false;

        }

        //Display stats page
        private void Button_Clicked_4(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Statistics(uEmail, uFirstName, uLastName, uID))
            {
                BarBackgroundColor = Color.LightPink
            };
            //hide menu bar
            IsPresented = false;
        }

        //Control sign out button
        private void Button_Clicked_5(object sender, EventArgs e)
        {
            //Set new current page to the login screen
            //This way if the user presses the back arrow, the app won't display the MasterPage again
            App.Current.MainPage = new MainPage();
        }
    }
}
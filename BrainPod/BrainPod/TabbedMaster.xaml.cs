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
    public partial class TabbedMaster : TabbedPage
    {
        public TabbedMaster(string userEmail, string userFirstName, string userLastName, Guid userID)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LogBtn.IsEnabled = false;

            userEmailDisplay.Text = userEmail;
            userFirstDisplay.Text = userFirstName;
            userLastDisplay.Text = userLastName;
            userIDDisplay.Text = userID.ToString();

            WelcomeMessage.Text = ("Welcome back " + userFirstName + "!");
            
        }

        bool SliderChanged = false;

        //handles a change in slider value
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SliderValue.Text = DayRatingSlider.Value.ToString();
            SliderChanged = true;
            
        }

       
        //handles editor text entry is complete
        private void JournalEntry_Completed(object sender, EventArgs e)
        {
            string journalEntry = JournalEntry.Text;
        }

        //checks if user has used slider
        private void JournalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if slider used then enable button to submit log as log has been entered and slider has been used
            if(SliderChanged == true && JournalEntry.Text != null)
            {
                LogBtn.IsEnabled = true;


            }

        }

        //store user data
        private void LogBtn_Clicked(object sender, EventArgs e)
        {
            string sliderVal = DayRatingSlider.Value.ToString();
            string journalVal = JournalEntry.Text;
        }
    }
}
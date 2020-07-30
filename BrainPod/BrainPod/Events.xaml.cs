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
    public partial class Events : ContentPage
    {
        //Globals
        int AnxietySliderValue = 0;
        int WorrySliderValue = 0;
        public Events(Guid uID)
        {
            InitializeComponent();

            CalendarLogo.Source = "https://cdn4.iconfinder.com/data/icons/small-n-flat/24/calendar-512.png";
            DatePicker.MinimumDate = DateTime.Now;
            DatePicker.MaximumDate = DateTime.Now.AddMonths(4);
            DatePicker.Format = "dd-MM-yy";
            AnxietyLevelDisplay.Text = "Anxiety level about event: ";
            WorryingThroughtLabel.Text = "Level of worry about event: ";
        }

        private void AnxietyLevelSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //get original values
            double originalValue = AnxietyLevelSlider.Value;
            //need to convert to int to remove decimal values
            AnxietySliderValue = Convert.ToInt32(originalValue);
            AnxietyLevelDisplay.Text = "Anxiety level about event: " + AnxietySliderValue;

        }

        private void WorryingThoughtSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //get original values
            double originalValue = WorryingThoughtSlider.Value;
            //need to convert to int to remove decimal values
            WorrySliderValue = Convert.ToInt32(originalValue);
            WorryingThroughtLabel.Text = "Level of worry about event: " + WorrySliderValue;
        }
    }
}
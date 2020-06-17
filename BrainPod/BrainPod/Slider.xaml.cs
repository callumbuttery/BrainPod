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
    public partial class Slider : ContentPage
    {
        public Slider()
        {
            InitializeComponent();
            //hide nav bar
            NavigationPage.SetHasNavigationBar(this, false);
            //used to load image
            LogoWithoutText.Source = ImageSource.FromFile("LogoWithoutText.png");
            ContinueBtn.IsEnabled = false;
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SliderValue.Text = DayRatingSlider.Value.ToString();
            ContinueBtn.IsEnabled = true;
        }
    }
}
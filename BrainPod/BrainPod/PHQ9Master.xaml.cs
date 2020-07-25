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
    public partial class PHQ9Master : ContentPage
    {
        //global
        Guid id;

        public PHQ9Master(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();
            SmileyIcon.Source = ImageSource.FromFile("smiley.png");
            id = uID;
        }

        public void startTestBtn(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PHQ9Test(id));
        }

        public void TestHistory(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PHQ9History(id));
        }
    }
}
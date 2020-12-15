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
    public partial class DistractionsMaster : ContentPage
    {
        public DistractionsMaster()
        {
            InitializeComponent();

            animationView.PlayAnimation();
        }

        private void LoadMaths(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DistractionsMaths());
        }

        private void LoadReaction(object sender, EventArgs e)
        {

        }

    }
}
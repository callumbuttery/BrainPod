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
    public partial class EventsHistory : ContentPage
    {
        //global
        Guid uID;

        public EventsHistory(Guid id)
        {
            InitializeComponent();

            uID = id;
        }
    }
}
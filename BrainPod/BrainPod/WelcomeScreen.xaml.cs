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
    public partial class WelcomeScreen : ContentPage
    {
        //Globals
        string email, firstName, lastName;
        Guid ID;
        public WelcomeScreen(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();
            email = uEmail;
            firstName = uFirstName;
            lastName = uLastName;
            ID = uID;

            //Welcome message with capitalised first name
            WelcomeMessage.Text = "Welcome to Brain Pod " + char.ToUpper(uFirstName[0]) + uFirstName.Substring(1) + "!";

            //set logo source
            Logo.Source = ImageSource.FromFile("Logo.png");


            //used to display quote on screen, each quote has a matching person that said the quote and their job title, both of which are in the arrays below this one
            string[] QuotesList = { "You’re off to great places, today is your day. Your mountain is waiting, so get on your way",
            "You always pass failure on the way to success",
            "No one is perfect - that’s why pencils have erasers",
            "You’re braver than you believe, and stronger than you seem, and smarter than you think",
            "Winning doesn’t always mean being first. Winning means you’re doing better than you’ve done before",
            "It always seems impossible until it is done",
            "Keep your face to the sunshine and you cannot see a shadow",
            "Once you replace negative thoughts with positive ones, you’ll start having positive results",
            "Positive thinking will let you do everything better than negative thinking will",
            "In every day, there are 1,440 minutes. That means we have 1,440 daily opportunities to make a positive impact",
            "It’s not whether you get knocked down, it’s whether you get up",
            "The way I see it, if you want the rainbow, you gotta put up with the rain",

            };


            string[] QuoteAuthor = { "Dr. Seuss",
                "Mickey Rooney",
                "Wolfgang Riebe",
                "A.A. Mine",
                "Bonnie Blair",
                "Nelson Mandela",
                "Helen Keller", 
                "Willie Nelson",
                "Zig Ziglar",
                "Les Brown",
                "Vince Lombardi",
                "Dolly Parton"};


            string[] AuthorJob = { "Author/Poet", 
                "Actor", 
                "Keynote Speaker/Magician", 
                "Author/Poet", 
                "Speed Skater", 
                "Political Leader", 
                "Author", 
                "Musician", 
                "Author", 
                "Author", 
                "Football player",
                "Singer-Songwriter"};

            //generate random number between 0 - 11 (1-12) to display random quote to screen
            Random rnd = new Random();
            int num = rnd.Next(0, 11);

            QuoteText.Text = "\"" +QuotesList[num] + "\"";
            QuoteAuthorText.Text = QuoteAuthor[num];
            AuthorJobText.Text = AuthorJob[num];
        }

    }
}
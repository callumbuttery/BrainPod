using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthScreen : ContentPage
    {
        //global information
        Guid ID;
        string auth;

        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");

        public AuthScreen(Guid userID, string authCode)
        {
            InitializeComponent();
            ID = userID;
            auth = authCode;

            //Logo.source is created in xaml file
            Logo.Source = ImageSource.FromFile("Logo.png");

        }

        //handle text change
        async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.NewTextValue == auth)
            {
                displayError.Text = "Valid";

                //update user as authenticated
                //fetch account based on information provided by user
                var getUser = (await firebaseClient
                    .Child("RegisteredUsers")
                    .OnceAsync<Table.RegisteredUsers>()).Where(a => a.Object.UserID == ID).FirstOrDefault();

                //values to update
                Guid uId = ID;
                string email = getUser.Object.Email;
                string password = getUser.Object.Password;
                string first = getUser.Object.FirstName;
                string last = getUser.Object.LastName;




                //delete value
               var delete = (await firebaseClient
                    .Child("RegisteredUsers")
                    .OnceAsync<Table.RegisteredUsers>()).Where(a => a.Object.UserID == getUser.Object.UserID).FirstOrDefault();
                await firebaseClient.Child("RegisteredUsers").Child(delete.Key).DeleteAsync();


              
                //post updated account details
                var result = await firebaseClient
                   .Child("RegisteredUsers")
                   .PostAsync(new Table.RegisteredUsers() { UserID = uId, Email = email, Password = password, FirstName = first, LastName = last, emailAuth = true });

                //redirect to homescreen
                Application.Current.MainPage = new MasterPage(result.Object.Email, result.Object.FirstName, result.Object.LastName, result.Object.UserID);
            }
            else
            {
                displayError.Text = "Invalid code";
            }
        }
    }
}
using BrainPod.Table;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountInfo : ContentPage
    {
        //new firebaseClient
        public static FirebaseClient firebaseClient = new FirebaseClient("https://brainpod-eba39.firebaseio.com/");
                //list to store logs returned from backend
        List<UserLogs> foundLogs = new List<UserLogs>();

        //global email
        string userEmail;
        string firstName;

        public AccountInfo(string uEmail, string uFirstName, string uLastName, Guid uID)
        {
            InitializeComponent();

            getLogs(uID);
            userEmailDisplay.Text = "Email: " + uEmail;
            userFirstDisplay.Text = "First name: " + uFirstName;
            userLastDisplay.Text = "Last name: " + uLastName;

            userEmail = uEmail;
            firstName = uFirstName;

        }

        async void getLogs(Guid userID)
        {

            /*getInstance will hold a list 
             * of log instances with the matching userID
             * */
            foundLogs = (await firebaseClient
            .Child("UserLogs")
            .OnceAsync<UserLogs>()).Where(a => a.Object.UserID == userID).Select(item => new UserLogs
            {
                UserID = item.Object.UserID,
                logData = item.Object.logData,
                sliderValue = item.Object.sliderValue,
                logTime = item.Object.logTime

            }).ToList();



            //Get all instances of recorded PHQ9 tests
            var getResults = (await firebaseClient
                .Child("phq9Results")
                .OnceAsync<phq9Results>()).Where(a => a.Object.UserID == userID).Select(item => new phq9Results
                {
                    UserID = item.Object.UserID,
                    overallResult = item.Object.overallResult,
                    submissionDate = item.Object.submissionDate

                }).ToList();

            //count number of instances
            int numberOfTests = getResults.Count;

        }

        //signout account
        private void SignOut_Clicked(object sender, EventArgs e)
        {
            //Set new current page to the login screen
            //This way if the user presses the back arrow, the app won't display the MasterPage again
            App.Current.MainPage = new MainPage();
            
           
            
        }

        //used to request password reset
        async void resetPassword(object sender, EventArgs e)
        {



            //generate email random auth number
            int min = 1000;
            int max = 9999;
            Random rnd = new Random();
            int auth = rnd.Next(min, max);

            try
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("callumbuttery@gmail.com");
                    mail.To.Add(userEmail);
                    mail.Subject = "BrainPod password reset";
                    mail.Body = "Hello " + firstName + " You have made a password reset request on brain pod!\n" + "<p>Please enter the following code into your app</p>" + "\n" + "Your email confirmation number is: " + auth;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("brainpod1234@gmail.com", "brainpodmailserver");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch(Exception error)
            {
                await DisplayAlert("Fail", error.ToString(), "Return");
                return;
            }

            try
            {
                await DisplayAlert("Success", "Please check your email for an email authentication number which you will need to enter to reset your password", "Return");

                var result = await DisplayPromptAsync("Security", "What's your email verification code?", keyboard: Keyboard.Numeric, maxLength: 4);

                if (Convert.ToInt32(result) == auth)
                {
                    //get input
                    var password = await DisplayPromptAsync("Security", "Please enter your new password?", keyboard: Keyboard.Text, maxLength: 16);

                    //loop
                    while (password == null || password.Length < 1 || password.Length > 16)
                    {
                        password = await DisplayPromptAsync("Security", "Please enter your new password?", keyboard: Keyboard.Text, maxLength: 16);
                    }

                    if (password != null)
                    {
                        //get account details to update user

                        //fetch account based on information provided by user
                        var getUser = (await firebaseClient
                            .Child("RegisteredUsers")
                            .OnceAsync<RegisteredUsers>()).Where(a => a.Object.Email == userEmail).FirstOrDefault();

                        string fname = getUser.Object.FirstName;
                        string lname = getUser.Object.LastName;
                        Guid uID = getUser.Object.UserID;
                        bool emailAuth = getUser.Object.emailAuth;
                        string email = getUser.Object.Email;

                        //delete account to post new updated details
                        await firebaseClient.Child("RegisteredUsers").Child(getUser.Key).DeleteAsync();

                        //add updated account
                        var post = await firebaseClient
                           .Child("RegisteredUsers")
                           //posts new user to databse
                           .PostAsync(new RegisteredUsers() { UserID = uID, Email = email, Password = password, FirstName = fname, LastName = lname, emailAuth = true });

                        await DisplayAlert("Success", "Password reset", "Return");

                        //sender user an email letting them know their password has been reset
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("brainpod1234@gmail.com");
                            mail.To.Add(userEmail);
                            mail.Subject = "Brain Pod password reset";
                            mail.Body = "Hello " + firstName + " You have successfully reset your password on Brain Pod\n" + "<p>Wasn't you? Please contact brainpod1234@gmail.com</p>";
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new System.Net.NetworkCredential("brainpod1234@gmail.com", "brainpodmailserver");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }
                        }

                    }

                }
            }
            catch (Exception xe)
            {
                await DisplayAlert("Fail", xe.ToString(), "Return");
            }
          
        }
    }
}
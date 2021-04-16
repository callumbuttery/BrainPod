using BrainPod.Table;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainPod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
          public static Firebase.Database.FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://brainpod-eba39.firebaseio.com/");

  
        public Registration()
        {
            InitializeComponent();
            //hide nav bar
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new RegisteredUsers();

          

        }

        //pop off current page from navigations stack to go back to login
        public void LoadLogin(object sender, EventArgs e)
        {
            //set new mainpage to login screen
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        //checks email that is entered is a valid email format
        public bool EmailValidator()
        {
            try
            {
                MailAddress mailAddress = new MailAddress(EmailInput.Text);
                return true;

            }
            catch
            {
                return false;
            }
        }

        //checks user has entered a password in a valid format e.g. bigger than 6 characters
        public bool PasswordValidator()
        {
            try
            {
                string pass = PasswordInput.Text;
                if (pass.Length > 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                //DisplayAlert("Password Validation", "Failed to validate password, please try again", "Retry");

                return false;
            }
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                //checks if email is registered to a user
                //returns either a null value or the user with the email
                var validuser = await CheckUserExists();

                //enters if user email account doesnt exist as null value has been returned
                if (validuser == null)
                {


                    //checks user has entered values into both entry boxes
                    if (FirstNameInput.Text == null || SecondNameInput.Text == null)
                    {
                        await DisplayAlert("Enter credentials", "Please enter a first name and second name", "Retry");
                        return;
                    }

                    //set to true or false value returned from email validation method
                    bool validEmail = EmailValidator();
                    //set to true or false value returned from password validation methid
                    bool passwordValidation = PasswordValidator();
                    if (validEmail == true && passwordValidation == true)
                    {
                        //Enable loading wheel
                        LoadingWheel.IsRunning = true;

                        Guid id = Guid.NewGuid();

                        //generate email random auth number
                        int min = 1000;
                        int max = 9999;
                        Random rnd = new Random();
                        int auth = rnd.Next(min, max);



                        //stores response from firebases
                        var result = await firebaseClient
                           .Child("RegisteredUsers")
                           //posts new user to databse
                           .PostAsync(new RegisteredUsers() { UserID = id, Email = EmailInput.Text, Password = PasswordInput.Text, FirstName = FirstNameInput.Text, LastName = SecondNameInput.Text, emailAuth = false, emailAuthCode = auth.ToString() });

                        //Hide loading wheel, no longer need to wait
                        LoadingWheel.IsRunning = false;

              
                        



                        if (result.Object != null)
                        {

                            using(MailMessage mail = new MailMessage())
                            {
                                mail.From = new MailAddress("brainpod1234@gmail.com");
                                mail.To.Add(EmailInput.Text);
                                mail.Subject = "Odro validation";
                                mail.Body = "Hello " + FirstNameInput.Text + " welcome to brain pod!\n" + "<p>Please enter the following code into your app</p>" + "\n" + "Your email confirmation number is: " + auth;
                                mail.IsBodyHtml = true;

                                using(SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtp.Credentials = new System.Net.NetworkCredential("brainpod1234@gmail.com", "brainpodmailserver");
                                    smtp.EnableSsl = true;
                                    smtp.Send(mail);
                                }
                            }

                            await DisplayAlert("Success", "Account successfully added\n\nPlease check your email for an email authentication number which you will need to enter next time you login","Return");
                            
                            
                            FirstNameInput.Text = null;
                            SecondNameInput.Text = null;
                            EmailInput.Text = null;
                            PasswordInput.Text = null;

                            Application.Current.MainPage = new NavigationPage(new MainPage());

                            
                        }
                        else
                        {
                            await DisplayAlert("Registration", "Failed registration, please try again", "Close");
                            return;

                        }
                    }
                    else
                    {
                        await DisplayAlert("Registration failure", "Please ensure you have entered a valid email and a password bigger than 6 characters", "Retry");
                        return;
                    }
                }
                else
                {
                    await DisplayAlert("Registration Failed", "Account with this email already exists", "Retry");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Failed", "The mouse fell off the wheel", "Retry");
                return;

            }
        }

        //used to check if user exists with the email provided
        private async Task<RegisteredUsers> CheckUserExists()
        {
            string email = EmailInput.Text;

            var getUser = (await firebaseClient
                .Child("RegisteredUsers")
                .OnceAsync<RegisteredUsers>()).Where(a => a.Object.Email == email).FirstOrDefault();

            if (getUser == null)
            {
                //await DisplayAlert("Login Failed", "Please check your email and password", "Return");
                return null;
            }
            else
            {
                var Content = getUser.Object as RegisteredUsers;
                return Content;


            }
        }
    }
}
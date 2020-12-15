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
    public partial class DistractionsMaths : ContentPage
    {
        //globals
        int calculationResult = 0;
        public DistractionsMaths()
        {
            InitializeComponent();

            //play animation
            mathsAnimation.PlayAnimation();

            generateNumbers();
        }

        public void generateNumbers()
        {
            
            
            //clear entry box
            numberInput.Text = null;

            //get all operators in array
            string[] operatorArray = { "X", "-", "+" };

            //generate first random number
            int min1 = 0005;
            int max1 = 0010;
            Random rnd1 = new Random();
            int random1 = rnd1.Next(min1, max1);

            //generate second random number
            int min2 = 0001;
            int max2 = 0005;
            Random rnd2 = new Random();
            int random2 = rnd2.Next(min2, max2);

            //generate random number between 0 and 3 to select and operator
            int min3 = 0000;
            int max3 = 0002;
            Random rnd3 = new Random();
            int random3 = rnd3.Next(min3, max3);


            //get operator
            operator1.Text = operatorArray[random3];

            //assign numbers
            no1.Text = random1.ToString();
            no2.Text = random2.ToString();

            //generate answer

            //calculation based on operator
            if(operatorArray[random3] == "X")
            {
                //multiply
                calculationResult = random1 * random2;
            }
            if (operatorArray[random3] == "-")
            {
                //subtract
                calculationResult = random1 - random2;
            }
            if (operatorArray[random3] == "+")
            {
                //multiply
                calculationResult = random1 + random2;
            }


        }

        private void check(object sender, EventArgs e)
        {
            //wrap in try catch incase user enters null value
            try
            {
                //stop animation
                animationView.StopAnimation();

                if (Convert.ToInt32(numberInput.Text) == calculationResult)
                {
                    //play celebration animation
                    animationView.IsVisible = true;
                    animationView2.IsVisible = false;
                    animationView.PlayAnimation();
                    
                    generateNumbers();
                    
                    

                }
                else
                {
                    //play loss animation
                    animationView.IsVisible = false;
                    animationView2.IsVisible = true;
                    animationView2.PlayAnimation();

                    //generate new numbers
                    generateNumbers();

                    
                }
            }
            catch
            {
                return;
            }
        }

        
    }
}
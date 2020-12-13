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

            generateNumbers();
        }

        public void generateNumbers()
        {
            //get all operators in array
            string[] operatorArray = { "X", "/", "-", "+" };

            //generate first random number
            int min1 = 0001;
            int max1 = 0020;
            Random rnd1 = new Random();
            int random1 = rnd1.Next(min1, max1);

            //generate second random number
            int min2 = 0001;
            int max2 = 0020;
            Random rnd2 = new Random();
            int random2 = rnd2.Next(min2, max2);

            //generate random number between 0 and 3 to select and operator
            int min3 = 0000;
            int max3 = 0003;
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
            if (operatorArray[random3] == "/")
            {
                //divide
                calculationResult = random1 / random2;
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

        public void check(object sender, EventArgs e)
        {
            //wrap in try catch incase user enters null value
            try
            {
                if(Convert.ToInt32(numberInput.Text) == calculationResult)
                {
                    //play celebration animation
                    //generate new numbers


                }
                else
                {
                    //play loss animation
                    //generate new numbers
                }
            }
            catch
            {
                return;
            }
        }

        
    }
}
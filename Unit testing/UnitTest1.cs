using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Unit_testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PHQ9_test()
        {
            int score = 0;
            string textCheck = "";

            string zerotofour = "Your test score suggests minimal to no symptoms or signs of depression";
            string fivetonine = "Your test score suggests you are experiencing mild symptoms related to depression which are unlikely to be causing disruption in your life";
            string tento14 = "Your test score suggests you are experiencing moderate symptoms related to depression which could be causing disruption in your life";
            string fifteento19 = "Your test score suggests you are experiencing moderately severe symtpoms related to depression which could be casuing disruption in your life";
            string twentyto27 = "Your test score suggests you are experiencing severe syptoms related to depression";


            //checks what data will display on each score

            while (score < 28)
            {
                //Depending on score, display certain information
                if (score >= 0 && score <= 4)
                {
                    textCheck = "Your test score suggests minimal to no symptoms or signs of depression";

                    
                }
                if (score >= 5 && score <= 9)
                {
                    textCheck = "Your test score suggests you are experiencing mild symptoms related to depression which are unlikely to be causing disruption in your life";
                  
                }
                if (score >= 10 && score <= 14)
                {
                    textCheck = "Your test score suggests you are experiencing moderate symptoms related to depression which could be causing disruption in your life";
                   
                }
                if (score >= 15 && score <= 19)
                {
                    textCheck = "Your test score suggests you are experiencing moderately severe symtpoms related to depression which could be casuing disruption in your life";
            
                }
                if (score >= 20 && score <= 27)
                {
                    textCheck = "Your test score suggests you are experiencing severe syptoms related to depression";

                }


                //check expected text and actual result are equal for each score
                if(score >= 0 && score <= 4)
                {
                    Assert.AreEqual(textCheck, zerotofour);
                }
                if (score >= 5 && score <= 9)
                {
                    Assert.AreEqual(textCheck, fivetonine);
                }
                if (score >= 10 && score <= 14)
                {
                    Assert.AreEqual(textCheck, tento14);
                }
                if (score >= 15 && score <= 19)
                {
                    Assert.AreEqual(textCheck, fifteento19);
                }
                if (score >= 20 && score <= 27)
                {
                    Assert.AreEqual(textCheck, twentyto27);
                }
                score = score + 1;
            }
        }
    }
}

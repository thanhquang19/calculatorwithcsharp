using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Assignment04_ThanhQPhan
{
    public partial class Calculator : Form
    {
        private double v1;

        private double v2;
        private string temp ="";
        private int signClickCount = 0; //to count how many times user clicks on Button_Neg, user may click the button multiple times 
        private string operation = "";
        private double result;
        private int decimalCount = 0;
        CalAdvanced Calculate = new CalAdvanced();
        SpeechSynthesizer spokenResult = new SpeechSynthesizer();
        private bool toSpeak = true;
        private bool toGetResult = false;
        private int operationClickCount = 0; //to track how many time user click +, - , *, /. the value is reset when user click equal or C
       

        public Calculator()
        {
            InitializeComponent();

        }
        private string SignDisplay(int count)
        {
            //if count is even which leads to -(-), the number is positive
            //if count is odd which leads to -(-)(-), the number is negative
            if (count % 2 == 0)
                return "";
            else
                return "-";
        }

        private void NumStringUpdate(string num)
        {
                temp = temp + num;
                Screen.Text = SignDisplay(signClickCount) + temp;
        }

        private void ValueReset ()
        {
            //after assign convert the input string to number, temp and signClickCount, and decimalCount are reset to the initial value
            //so that the program can take the input after user clicks operation buttons as the second value

            temp = "";
            signClickCount = 0;
            decimalCount = 0;
        }
        private bool IsNegative(int count)
        {
            if (count % 2 == 0)
                return false;
            else
                return true;
        }

        private  double ConvertStringToNumber(double value, int count, string numstring)
        //value is always initialized
        //the value is included in the parameter because user may change the operation.
        //because the temp and count are reset each time user click on the operation
        //including the value here to prevent the crash or loss
        {
            if (numstring.Length != 0 && numstring != ".")
            {
                if (IsNegative(count))
                {
                    value = -Convert.ToDouble(numstring);
                }
                else
                {
                    value = Convert.ToDouble(numstring);
                }

                return value;
            }
            else
            {
                return value;

            } 
        }
        private void OperationUpdate(string ops)
        {
            if (Screen.Text != "Math Error!")
            {
                if(operationClickCount <= 1) 
                {
                    operation = ops;
                    v1 = ConvertStringToNumber(v1,signClickCount,temp);
                    ValueReset();
                    Screen.Text = operation;
                }
                else if(operationClickCount > 1) 
                    //this is the where user continues to click another operation instead of click equal
                    //the calculator performs the previous operation and assign the result to v1 
                {
                    EqualUpdate();
                    operation = ops;
                    v1 = ConvertStringToNumber(v1,signClickCount, temp);
                    ValueReset();
                    Screen.Text = operation;
                }
                toGetResult = false; //false so that CE will not erase all the value in case of continuing operations
            }
            else
            {
                throw new ArgumentException();
            }
                   
        }
        private void EqualUpdate ()
        {
            string spokenOps = "";
            v2 = ConvertStringToNumber(v1,signClickCount,temp);
            //if user clicks equal sign right after the operation without inputting v2 value
            //use v1 as v2 as window calculator does
            ValueReset();
            if (operation == "")
            //in case user dont input operation
            {
                result = v2;
            }
            else if (operation == "+")
            {
                result = Calculate.Add(v1, v2);
                spokenOps = "plus";
            }
            else if (operation == "-")
            {
                result = Calculate.Substract(v1, v2);
                spokenOps = "minus";
            }
            else if (operation == "*")
            {
                result = Calculate.Multiply(v1, v2);
                spokenOps = "multiplied by";
            }
            else if (operation == "/")
            {
                result = Calculate.Divide(v1, v2);
                spokenOps = "divided by";
            }

            if (toSpeak == true)
            {
                if (operation != "")
                    SpokenSpeech($"{v1} {spokenOps} {v2} equals {result}");
                else
                    SpokenSpeech($"{v2} is {result}");
            }

            ResultToString();
        }
        private void ResultToString ()
        {
            v1 = result; //assign result to v1 for further calculation
            operation = "";
            Screen.Text = result.ToString();
        }

        private void SpokenSpeech(string calResult)
        {
            spokenResult.Volume = 75;
            spokenResult.Rate = -1;
            spokenResult.SelectVoiceByHints(VoiceGender.Female);
            spokenResult.SpeakAsync(calResult);
        }
        
        private void Button_0_Click(object sender, EventArgs e)
        {
            NumStringUpdate("0");
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            NumStringUpdate("1");
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            NumStringUpdate("2");
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            NumStringUpdate("3");
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            NumStringUpdate("4");
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            NumStringUpdate("5");
        }

        private void button_6_Click(object sender, EventArgs e)
        {
            NumStringUpdate("6");
        }

        private void button_7_Click(object sender, EventArgs e)
        {
            NumStringUpdate("7");
        }

        private void button_8_Click(object sender, EventArgs e)
        {
            NumStringUpdate("8");
        }

        private void button_9_Click(object sender, EventArgs e)
        {
            NumStringUpdate("9");
        }

        private void button_Dec_Click(object sender, EventArgs e)
        {
            decimalCount++;
            if(decimalCount <= 1) //there is no more than 1 decimal point in a number
            {
                temp = temp + ".";
            }
            
            Screen.Text =  SignDisplay(signClickCount) + temp;
        }

        private void button_BackSpace_Click(object sender, EventArgs e)
        {
            try
            {
                temp = temp.Remove(temp.Length - 1, 1);
                Screen.Text =  SignDisplay(signClickCount) + temp;
                if(temp.Contains(".") == false)
                {
                    decimalCount = 0; //reset decimal count in case user click backspace to remove current decimal point
                }
            }
            catch
            {
                //try-catch here to prevent user click the button prior to inputting any value
            }
        }
        private void button_Neg_Click(object sender, EventArgs e)
        {
            signClickCount++;
            
            if (temp=="" && operation=="") //the scenario when user continues to perform calculation on the result
            {
                if (result < 0)
                {
                    Screen.Text = SignDisplay(signClickCount+1) + result.ToString().Remove(0, 1); //if negative of negative makes a positive
                    v1 = ConvertStringToNumber(v1, signClickCount + 1, result.ToString().Remove(0, 1));
                    
                }
                else
                {
                    Screen.Text = SignDisplay(signClickCount) + result.ToString();
                    v1 = ConvertStringToNumber(v1, signClickCount, result.ToString());
                }

                signClickCount = 0;
            }
            else
            {
                Screen.Text = SignDisplay(signClickCount) + temp;
            }

        }
                
        private void button_Add_Click(object sender, EventArgs e)
        {
            try
            {
                operationClickCount++;
                OperationUpdate("+");
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
             
        }

        private void button_Substract_Click(object sender, EventArgs e)
        {
            try
            {
                operationClickCount++;
                OperationUpdate("-");
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
        }

        private void button_Multiply_Click(object sender, EventArgs e)
        {
            try
            {
                operationClickCount++;
                OperationUpdate("*");
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
        }

        private void button_Divide_Click(object sender, EventArgs e)
        {
            try
            {
                operationClickCount++;
                OperationUpdate("/");
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
        }
      
        private void button_Per_Click(object sender, EventArgs e)
        {
            try {
                toGetResult = true;
                OperationUpdate("%");
                result = Calculate.Percentage(v1, v2);
                if (toSpeak == true)
                {
                    SpokenSpeech($"one percent of {v1} is {result}");
                }
                ResultToString();
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
        }

        private void button_Sqroot_Click(object sender, EventArgs e)
        {            
            try {
                toGetResult = true;
                OperationUpdate("sqrt"); 
                result = Calculate.SquareRoot(v1, v2);
                if(toSpeak == true)
                {
                    SpokenSpeech($"Square root of {v1} is {result}");
                }
                ResultToString();
            } //the try-catch to prevent square root of a negative number
            catch { Screen.Text = "Math Error!";}

        }

        private void button_Square_Click(object sender, EventArgs e)
        {
            try
            {
                toGetResult = true;
                OperationUpdate("sqr");
                result = Calculate.Square(v1, v2);
                if (toSpeak == true)
                {
                    SpokenSpeech($"{v1} squared is {result}");
                }
                ResultToString();
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
          

        }

        private void button_Fraction_Click(object sender, EventArgs e)
        {
            
            try
            {
                toGetResult = true;
                OperationUpdate("1/x");
                result = Calculate.Fraction(v1, v2);
                if (toSpeak == true)
                {
                    SpokenSpeech($"1 divided by {v1} is {result}");
                }
                ResultToString();
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
            
        }

        private void button_Equal_Click(object sender, EventArgs e)
        { 
            try
            {
                toGetResult = true;
                EqualUpdate();
                operationClickCount = 0;
            }
            catch
            {
                Screen.Text = "Math Error!";
            }
        }

        private void button_C_Click(object sender, EventArgs e)
        {
            v1 = 0;
            v2 = 0;
            result = 0;
            toGetResult = false;
            operation = "";
            operationClickCount = 0;
            ValueReset();
            Screen.Text = " ";
        }

        private void button_CE_Click(object sender, EventArgs e) 
            //keep the last display value in memory
            //set user input to the intialized value 
            //there will be two scenario as observing Windown Calculator
        {
            if (toGetResult == true)
                //the scenario where the calculation is perform and result is given
                //act same as C button
                button_C_Click(sender, e);
            else
            {  //if not, only clear the display
                ValueReset();
                Screen.Text = "";
            }
           
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_C_Click(sender, e);
        }

        private void cEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_CE_Click(sender, e);
        }

        private void equalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_Equal_Click(sender, e);
        }

        private void yesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toSpeak = true;
        }

        private void noToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toSpeak = false;
        }
    }
}

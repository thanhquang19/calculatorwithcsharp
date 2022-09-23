using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04_ThanhQPhan
{
    class CalAdvanced: BaseMath
    {
        public double Percentage(double v1, double v2)
        {
            //v2 is not required, it's disregarded by the following operation
            return v1 /100;
        }

        public double Square(double v1, double v2)
        {
            //v2 is not required, it's disregarded by the following operation
            return v1*v1;
        }

        public double SquareRoot(double v1, double v2)
        {
            //v2 is not required, it's disregarded by the following operation


            if (v1 >= 0) // square root is only applicable for non-negative numner
            {
                double testValue = 1; //initialize a testing value
                int i = 0;

                if (v1 == 0) //square root of 0 is 0
                    return 0;

                while (true) 
                {
                    i += 1; 
                    testValue = (v1 / testValue + testValue) / 2;
                    if (i == v1 + 1 || i == 100) //loop ends when number of testing exceeds the value or it reaches 100
                        break;
                }

                return testValue;
            }

            else
                throw new ArgumentException();
        }

        public double Fraction(double v1, double v2)
        {
            //v2 is not required, it's disregarded by the following operation 
            if (v1 != 0)
                return 1 / v1;
            else
                throw new ArgumentException();
        }

      

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04_ThanhQPhan
{
    class BaseMath
    {
        public BaseMath()
        {

        }
        public double Add(double v1, double v2)
        {
            return v1 + v2;
        }

        public double Substract(double v1, double v2)
        {
            return v1 - v2;
        }
        public double Multiply(double v1, double v2)
        {
            return v1 * v2;
           
        }
        public double Divide(double v1, double v2)
        {
            if (v2 != 0) 
                return v1/v2;
            else
                throw new ArgumentException();
        }

       
    }
}

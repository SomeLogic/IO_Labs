using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// This class is responsible for mathematical operations.
    /// Its functions are static so you don't need to create a class object.
    /// </summary>
    public class Calculator
    {
        public static int AddNumbers(int num1, int num2)
        {
            return num1 + num2;
        }
        /// <summary>
        /// Substraction is on unsigned numbers so we can't get negative result.
        /// If num1 is greater than num2, function returns 0.
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static int SubNumbers(int num1, int num2)
        {
            if (num1 < num2) return 0;
            return num1 - num2;
        }
        public static int MulNumbers(int num1, int num2)
        {
            return num1 * num2;
        }
        public static int DivNumbers(int num1, int num2)
        {
            return num1 / num2;
        }
    }
}

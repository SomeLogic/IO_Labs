using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// This class is responsible for parsing input string to get numbers and mathematical operation.
    /// </summary>
    public class Interpreter
    {
        private String msg;
        private int num1, num2, result;
        private string op;
        private bool isNum1Set = false, isNum2Set = false, isOpSet = false;
        public Interpreter(String msg) { this.msg = msg; readData(); }

        /// <summary>
        /// This function parses string and writes values to variables.
        /// After that you can get these values by using getter functions.
        /// If string has wrong format, an error string is written to op variable.
        /// </summary>
        private void readData()
        {
            if (msg == "his")
            {
                op = "history";
                return;
            }
            if (msg.Length != 3)
            {
                op = "Error";
                return;
            }
            for(int i = 0; i < msg.Length; i++)
            {
                if (!isNum1Set)
                {
                    if (msg.ElementAt(i) >= 0x30 && msg.ElementAt(i) <= 0x39)
                    {
                        num1 = (int)char.GetNumericValue(msg.ElementAt(i));
                        isNum1Set = true;
                    }
                    else
                    {
                        op = "Error";
                        break;
                    }
                }
                else if (!isOpSet)
                {
                    if (msg.ElementAt(i) == '+' || msg.ElementAt(i) == '-' || msg.ElementAt(i) == '*' || msg.ElementAt(i) == '/')
                    {
                        op = msg.ElementAt(i).ToString();
                        isOpSet = true;
                    }
                    else
                    {
                        op = "Error";
                        break;
                    }
                }
                else if (!isNum2Set)
                {
                    if (msg.ElementAt(i) >= 0x30 && msg.ElementAt(i) <= 0x39)
                    {
                        num2 = (int)char.GetNumericValue(msg.ElementAt(i));
                        isNum2Set = true;
                    }
                    else
                    {
                        op = "Error";
                        break;
                    }
                }
            }
        }

        public string getOp()  { return op;   }
        public int getNum1() { return num1; }
        public int getNum2() { return num2; }
        public int getResult() { return result; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace ServerTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipaddr = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(ipaddr, 9999);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                NetworkStream ns = client.GetStream();

                while (client.Connected)
                {
                    byte[] msg = new byte[3];
                    ns.Read(msg, 0, msg.Length);
                    String str = Encoding.Default.GetString(msg);
                    ClassLibrary.Interpreter interpreter = new ClassLibrary.Interpreter(str);
                    if (interpreter.getOp() == "Error" && str != "\r\n\0")
                    {
                        Console.WriteLine("Can't read input as a math equation :c\n");
                        byte[] error = new byte[100];
                        error = Encoding.Default.GetBytes("Can't read input as a math equation :c\n"); 

                        ns.Write(error, 0, error.Length);
                    }
                    else if (str != "\r\n\0")
                    {
                        char op = interpreter.getOp().ElementAt(0);
                        int num1 = interpreter.getNum1(), num2 = interpreter.getNum2();
                        int result = 0;
                        switch (op)
                        {
                            case '+':
                                result = ClassLibrary.Calculator.AddNumbers(num1, num2);
                                Console.WriteLine(str + "=" + ClassLibrary.Calculator.AddNumbers(num1, num2));
                                break;
                            case '-':
                                result = ClassLibrary.Calculator.SubNumbers(num1, num2);
                                Console.WriteLine(str + "=" + ClassLibrary.Calculator.SubNumbers(num1, num2));
                                break;
                            case '*':
                                result = ClassLibrary.Calculator.MulNumbers(num1, num2);
                                Console.WriteLine(str + "=" + ClassLibrary.Calculator.MulNumbers(num1, num2));
                                break;
                            case '/':
                                result = ClassLibrary.Calculator.DivNumbers(num1, num2);
                                Console.WriteLine(str + "=" + ClassLibrary.Calculator.DivNumbers(num1, num2));
                                break;
                            default: 
                                Console.WriteLine("Something went wrong.");
                                break;
                        }
                        if (result > 9)
                        {
                            byte[] res = new byte[4];
                            res = Encoding.Default.GetBytes(result.ToString());
                            ns.Write(res, 0, res.Length);
                        }
                        else
                        {
                            byte[] res = new byte[1];
                            res = Encoding.Default.GetBytes(result.ToString());
                            ns.Write(res, 0, res.Length);
                        }
                    }
                }
            }
        }
    }
}

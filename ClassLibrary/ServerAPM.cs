using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClassLibrary
{
    public class ServerAPM : AbstractServer
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);
        public ServerAPM(IPAddress IP, int port) : base(IP, port)
        {

        }
        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);
                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            Hello(stream);
            string history = "History:\r\n";

            byte[] buffer = new byte[Buffer_size];

            while (true)
            {
                try
                {
                    byte[] msg = new byte[3];
                    stream.Read(msg, 0, msg.Length);
                    String str = Encoding.Default.GetString(msg);
                    ClassLibrary.Interpreter interpreter = new ClassLibrary.Interpreter(str);
                    if (interpreter.getOp() == "Error" && str != "\r\n\0")
                    {
                        Console.WriteLine("Can't read input as a math equation :c\n");
                        byte[] error = new byte[100];
                        error = Encoding.Default.GetBytes("Wrong input data, please use format DoD where D is digit and o is operation (+, -, *, /)\r\n");

                        stream.Write(error, 0, error.Length);
                    }
                    else if (str != "\r\n\0")
                    {
                        if(interpreter.getOp() == "history")
                        {
                            byte[] str_final_msg = new byte[1024];
                            str_final_msg = Encoding.Default.GetBytes(history);
                            stream.Write(str_final_msg, 0, history.Length);
                        }
                        else
                        {
                            char op = interpreter.getOp().ElementAt(0);
                            int num1 = interpreter.getNum1(), num2 = interpreter.getNum2();
                            int result = 0;
                            string str_final_eq = "";
                            switch (op)
                            {
                                case '+':
                                    result = ClassLibrary.Calculator.AddNumbers(num1, num2);
                                    str_final_eq = str + "=" + ClassLibrary.Calculator.AddNumbers(num1, num2) + "\r\n";
                                    break;
                                case '-':
                                    result = ClassLibrary.Calculator.SubNumbers(num1, num2);
                                    str_final_eq = str + "=" + ClassLibrary.Calculator.SubNumbers(num1, num2) + "\r\n";
                                    break;
                                case '*':
                                    result = ClassLibrary.Calculator.MulNumbers(num1, num2);
                                    str_final_eq = str + "=" + ClassLibrary.Calculator.MulNumbers(num1, num2) + "\r\n";
                                    break;
                                case '/':
                                    result = ClassLibrary.Calculator.DivNumbers(num1, num2);
                                    str_final_eq = str + "=" + ClassLibrary.Calculator.DivNumbers(num1, num2) + "\r\n";
                                    break;
                                default:
                                    Console.WriteLine("Something went wrong.");
                                    break;
                            }
                            if (result != 0)
                            {
                                byte[] str_final_msg = new byte[256];
                                str_final_msg = Encoding.Default.GetBytes(str_final_eq);
                                stream.Write(str_final_msg, 0, str_final_eq.Length);
                                history += str_final_eq;
                            }
                        }
                    }
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }

        public void Hello(NetworkStream stream)
        {
            byte[] hello_msg = new byte[256];
            string hello_str = "Welcome to my remote calculator!\r\n";
            hello_msg = new ASCIIEncoding().GetBytes(hello_str);
            stream.Write(hello_msg, 0, hello_str.Length);
            
            hello_str = "Feel free to type 3 characters, please use format DoD where D is digit and o is operation (+, -, *, /), \'his\' - history of your calcuations.\r\n";
            hello_msg = new ASCIIEncoding().GetBytes(hello_str);
            stream.Write(hello_msg, 0, hello_str.Length);
        }
    }
}
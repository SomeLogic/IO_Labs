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
            ClassLibrary.ServerAPM server = new ClassLibrary.ServerAPM(IPAddress.Parse("127.0.0.1"), 9999);
            try
            {
                server.Start();
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

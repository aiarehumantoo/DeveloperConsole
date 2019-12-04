﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// This is the server, Unity is the client
namespace DeveloperConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(iep);
            server.Listen(10);
            Console.WriteLine("Waiting for connection...");
            using (Socket client = server.Accept()) // Waits for connection before proceeding
            {
                while (true)
                {
                    string s = Console.ReadLine().ToUpper();

                    if (s.Equals("QUIT"))
                    {
                        break;
                    }

                    // Do not send line if empty
                    if (!s.Equals(""))
                    {
                        // Send string
                        byte[] send_buffer = Encoding.ASCII.GetBytes(s);
                        client.SendTo(send_buffer, iep);

                        //Console.WriteLine("Send success!");
                    }

                    if (s.ToUpper().Equals("HELP"))
                    {
                        //Console.WriteLine("\n" + "Command list:");
                        //Console.WriteLine("Command1");
                        //Console.WriteLine("Command2");

                        while (true)
                        {
                            Console.WriteLine("Waiting for reply...");
                            byte[] buffer = new byte[64]; // Max string size. New line if is exceeded
                            client.Receive(buffer, buffer.Length, SocketFlags.None);
                            var messageString = Encoding.ASCII.GetString(buffer);
                            Console.WriteLine("\n" +"List of console commands: \n" + messageString);
                            break;
                        }
                    }
                }
            }
        }
    }
}

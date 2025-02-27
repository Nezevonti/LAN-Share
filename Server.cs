﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LAN_Share
{
    class Server
    {
        /*
        // Main Method 
        static void Main(string[] args)
        {
            ExecuteServer();
        }
        */

        static String DesktopIP = "192.168.0.26";
        static String LaptopIP = "192.168.0.52";

        public Server()
        {
            //ExecuteServer();
            ExcecuteServerBroadcastHandler();
            Console.ReadKey();
        }


        public static void ExcecuteServerBroadcastHandler()
        {

            Socket brSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11111);

            brSocket.Bind(endPoint);
            EndPoint endPointRemote = (EndPoint)endPoint;

            Console.WriteLine("Waiting for broadcast");

            byte[] brMsg = new byte[1024];

            brSocket.ReceiveFrom(brMsg,ref endPointRemote);

            String BrData = Encoding.ASCII.GetString(brMsg);

            Console.WriteLine(BrData);

            brSocket.Close();


            Console.ReadKey();
        }

        public static void ExecuteServer()
        {
            // Establish the local endpoint  
            // for the socket. Dns.GetHostName 
            // returns the name of the host  
            // running the application.

            //IPHostEntry ipHost = Dns.GetHostEntry("DESKTOP-1GC20RS");
            //IPAddress ipAddr = ipHost.AddressList[ipHost.AddressList.Length - 1];

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11111);

            // Creation TCP/IP Socket using  
            // Socket Class Costructor 
            Socket listener = new Socket(IPAddress.Any.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a 
                // network address to the Server Socket 
                // All client that will connect to this  
                // Server Socket must know this network 
                // Address 
                listener.Bind(localEndPoint);

                // Using Listen() method we create  
                // the Client list that will want 
                // to connect to Server 
                listener.Listen(10);

                while (true)
                {

                    Console.WriteLine("Waiting connection ... ");

                    // Suspend while waiting for 
                    // incoming connection Using  
                    // Accept() method the server  
                    // will accept connection of client 
                    Socket clientSocket = listener.Accept();

                    // Data buffer 
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {

                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                                   0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);
                    byte[] message = Encoding.ASCII.GetBytes("Test Server");

                    // Send a message to Client  
                    // using Send() method 
                    clientSocket.Send(message);

                    // Close client Socket using the 
                    // Close() method. After closing, 
                    // we can use the closed Socket  
                    // for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

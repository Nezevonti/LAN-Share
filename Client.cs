using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LAN_Share
{
    class Client
    {
        /*
        // Main Method 
        static void Main(string[] args)
        {
            ExecuteClient();
        }
        */

        static String DesktopIP = "192.168.0.26";
        static String LaptopIP = "192.168.0.52";

        public Client()
        {
            bool Continue = true;

            while (Continue)
            {
                //ExecuteClient();
                ExecuteBroadcast();
                if(Console.ReadKey().Key == ConsoleKey.E)
                {
                    Continue = false;
                }
            }
            
        }

        static String GetMessage()
        {
            return Console.ReadLine()+"<EOF>";
        }


        // ExecuteBroadcast() Method
        static void ExecuteBroadcast()
        {
            try
            {
                String DiscoveryMsg = "I wanna send you a file. What is your IP?<EOF>";
                byte[] MsgSent = Encoding.ASCII.GetBytes(DiscoveryMsg);

                IPEndPoint EndPoint = new IPEndPoint(IPAddress.Broadcast, 11111);

                Socket bcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                bcSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                bcSocket.ReceiveTimeout = 200;

                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress hostAddress = hostEntry.AddressList[hostEntry.AddressList.Length - 1];
                IPEndPoint localEndPoint = new IPEndPoint(hostAddress, 11111);
                bcSocket.Bind(localEndPoint);

                while (true)
                {
                    bcSocket.SendTo(MsgSent, EndPoint);

                    Console.WriteLine("Msg Sent");

                    System.Threading.Thread.Sleep(1000);
                }

                


            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }



        // ExecuteClient() Method 
        static void ExecuteClient()
        {

            try
            {

                // Establish the remote endpoint  
                // for the socket. This example  
                // uses port 11111 on the local  
                // computer. 
                IPHostEntry ipHost = Dns.GetHostEntry(IPAddress.Parse(DesktopIP));
                IPAddress ipAddr = ipHost.AddressList[ipHost.AddressList.Length-1];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                // Creation TCP/IP Socket using  
                // Socket Class Costructor 
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Connect Socket to the remote  
                    // endpoint using method Connect() 
                    sender.Connect(localEndPoint);

                    // We print EndPoint information  
                    // that we are connected 
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creation of messagge that 
                    // we will send to Server
                    String Msg = GetMessage();
                    byte[] messageSent = Encoding.ASCII.GetBytes(Msg);
                    int byteSent = sender.Send(messageSent);

                    // Data buffer 
                    byte[] messageReceived = new byte[1024];

                    // We receive the messagge using  
                    // the method Receive(). This  
                    // method returns number of bytes 
                    // received, that we'll use to  
                    // convert them to string 
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(messageReceived,
                                                     0, byteRecv));

                    // Close Socket using  
                    // the method Close() 
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}

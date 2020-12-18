using System;
using System.Net;

namespace LAN_Share
{
    class Program
    {
        
        static void Main(string[] args)
        {
            String input;

            /*
            String DesktopIP = "192.168.0.26";
            String LaptopIP = "192.168.0.52";

            IPHostEntry host = Dns.GetHostEntry("DESKTOP-1GC20RS");

            Console.WriteLine($"GetHostEntry({"192.168.0.26"}) returns:");

            foreach (IPAddress address in host.AddressList)
            {
                Console.WriteLine($"    {address}");
            }
            */

            
            Console.WriteLine("For server type 'server', for client type 'client'");
            input = Console.ReadLine();

            if(input == "server")
            {
                Server S = new Server();
            }
            if(input == "client")
            {
                Client C = new Client();
            }
            
        }
    }
}

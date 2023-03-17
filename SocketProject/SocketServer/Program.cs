using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an object of Socket Class
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Create an object of an IP Address.Socket listening on any ip Address
            IPAddress ipAddr = IPAddress.Any;

            // Define IP ENDP POINT
            IPEndPoint ipep = new IPEndPoint(ipAddr, 25000);

            // Bind socket to up end point
            try 
            {
                listenerSocket.Bind(ipep);

                // Call listen method on the listener socket object, pass a number to signify how many
                // Clients can wait for a connection while the system is busy handling another connection
                listenerSocket.Listen(5);

                // Call accept method on the listener socket.
                // Accept is a synchronous method (blocking method() will not move forward until operation completed -- very bad method
                Socket client = listenerSocket.Accept();

                // print out client ip end point
                Console.WriteLine("Client connected: " + client.ToString() + " - IP End Point: " + client.RemoteEndPoint.ToString());

                // Define buffer as type array
                byte[] buffer = new byte[128];

                // Define number of received bytes as an int
                int numberOfReceivedBytes = 0;
                
                while (true)
                {
                    numberOfReceivedBytes = client.Receive(buffer);
                    Console.WriteLine("Number of received bytes: " + numberOfReceivedBytes);

                    // Convert from byte array to ascii , using encoding.ASCII.GetString
                    string receivedText = Encoding.ASCII.GetString(buffer, 0, numberOfReceivedBytes);

                    // print out text
                    Console.WriteLine("Data sent by client is: " + receivedText);

                    // send data back to sender
                    client.Send(buffer);

                    if (receivedText == "x")
                    {
                        break;
                    }

                    Array.Clear(buffer, 0, numberOfReceivedBytes);
                    numberOfReceivedBytes = 0;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

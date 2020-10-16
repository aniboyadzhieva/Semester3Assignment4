
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MandatoryAssignment1;

namespace TcpServerBook
{
    public class HandleClient
    {
        private static List<Book> bList = new List<Book>()
        {
            new Book("Smth", "Smth", 600, "1111111111111"),
            new Book("Smth2", "Smth2", 650, "2222222222222"),
            new Book("Smth3", "Smth3", 700, "1111111111113"),
        };

        TcpClient clientSocket;

        public void startClient(TcpClient inClientSocket)
        {
            this.clientSocket = inClientSocket;

            Thread ctThread = new Thread(doSth);
            ctThread.Start();
        }

        private static Stream _nStream;
        private static StreamWriter _sWriter;
        private static StreamReader _sReader;

        private void doSth()
        {
            //client IP address
            var clientIp = ((IPEndPoint) clientSocket.Client.RemoteEndPoint).Address;
            //client port number
            var clientPort = ((IPEndPoint) clientSocket.Client.RemoteEndPoint).Port;

            Console.WriteLine("Client has an IP: " + clientIp + "Port" + clientPort);
            Console.WriteLine("Now handshake has been created");
            Console.WriteLine("Client and server are able to communicate over the networks");

            using (_nStream = clientSocket.GetStream())
            {
                //_sWriter = new StreamWriter(_nStream)
                //{
                //    AutoFlush = true
                //};
                _sReader = new StreamReader(_nStream);
                _sWriter = new StreamWriter(_nStream) {AutoFlush = true};

                //step 1 - read message from client
                //var msgFromClient = _sReader.ReadLine();
                var methodFromClient = _sReader.ReadLine();
                var paramFromClient = _sReader.ReadLine();
                while (true)
                {
                    if (methodFromClient == "GetAll")
                    {
                        foreach (var b in bList)
                        {
                            Console.WriteLine("Title: " + b.Title + "Author: " + b.Author);
                        }
                    }
                    else if (methodFromClient == "Get")
                    {
                        bList.Find(x => x.Isbn13 == paramFromClient);
                    }
                    //Console.WriteLine("Client message: " + msgFromClient);

                    //respond back to client
                    var msgFromServer = Console.ReadKey();
                    _sWriter.WriteLine(msgFromServer);
                }
            }
        }
    }
}

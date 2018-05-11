using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPXilicaSimulator
{
    public class ClientHandler
    {
        private readonly Thread _thread;
        private readonly byte[] _buffer = new byte[1024];
        private const int BufferSize = 1024;
        private readonly TcpClient _tcpclient;
        private readonly NetworkStream _networkStream;
        private List<byte> _totalBuffer;

        public ClientHandler(TcpClient client)
        {
            _tcpclient = client;

            _networkStream = _tcpclient.GetStream();
            _totalBuffer = new List<byte>();
            _thread = new Thread(ThreadLoop);
            _thread.Start();

        }

        private void ThreadLoop()
        {
            while (true)
            {
                try
                {
                    //Is the client connected?
                    if (!_tcpclient.Connected)
                        throw new SocketException(0xDC);

                    //Recieve the data from the networkStream
                    var receiveCount = _networkStream.Read(_buffer, 0, BufferSize);

                    var rawData = new byte[receiveCount];
                    Array.Copy(_buffer, 0, rawData, 0, receiveCount);
                    _totalBuffer = _totalBuffer.Concat(rawData).ToList();

                    //Check the packetsize, did we recieve anything?
                    var packetSize = Packet.GetLengthOfPacket(_totalBuffer);
                    if (packetSize == -1)
                        continue;

                    //Retrieve the Json out of the received packet.
                    JObject json = null;

                    string s = Packet.RetriveString(_totalBuffer.Count, ref _totalBuffer);
                    Console.WriteLine(s);
                    Send("acknowledged");
                    switch (s)
                    {
                        case "SETRAW mutemic1 0":
                            break;
                        case "SETRAW mutemic1 1":
                            break;
                        case "SETRAW mutemic2 0":
                            break;
                        case "SETRAW mutemic2 1":
                            break;
                        case "SETRAW mutemic3 0":
                            break;
                        case "SETRAW mutemic3 1":
                            break;
                        case "SETRAW mutemic4 0":
                            break;
                        case "SETRAW mutemic4 1":
                            break;
                        case "SETRAW declamatie 0":
                            break;
                        case "SETRAW declamatie 1":
                            break;
                        case "SETRAW volumespraak+ 1":
                            break;
                        case "SETRAW volumespraak- 1":
                            break;
                        case "SETRAW volumemuziek+ 1":
                            break;
                        case "SETRAW volumemuziek- 1":
                            break;

                    }

                }
                catch (SocketException)
                {
                    Console.WriteLine("Client with IP-address: {0} has been disconnected",
                    _tcpclient.Client.RemoteEndPoint);
                    _thread.Abort();
                }
                catch (Exception e)
                {
                    if (e.InnerException is SocketException)
                    {
                        Console.WriteLine("Client with IP-address: {0} has been disconnected",
                        _tcpclient.Client.RemoteEndPoint);
                        _thread.Abort();
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        #region Packet_Handlers
        

        #endregion

        #region Send_methods
        

        public void Send(String s)
        {
            var dataArray = Packet.CreateByteData(s);
            _networkStream.Write(dataArray, 0, dataArray.Length);
            _networkStream.Flush();
        }
        
        #endregion Send
    }

}

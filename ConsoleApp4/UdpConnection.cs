using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ConsoleApp4
{
    public class UdpConnection : IConnection
    {
        private UdpClient client = null;
        private IPEndPoint gropuEP = null;

        public static int PacketSize = 512;
        public static int ListenPort = 12345;

        private Thread worker = null;

        private object locker = new object();

        private bool isRunning = false;

        private string message;

        public string Message
        {
            get
            {
                lock (locker)
                {
                    return message;
                }
            }

            set
            {
                lock (locker)
                {
                    message = value;
                }
            }
        }

        private void Run()
        {

            for (; ; )
            {
                lock (locker)
                {
                    if (!isRunning)
                    {
                        break;
                    }
                }

                byte[] data = new byte[PacketSize];

                {
                    data = client.Receive(ref gropuEP);
                    if (data.Count() > 0)
                    {
                        Message = string.Format("{0}", Encoding.ASCII.GetString(data, 0, data.Length));
                    }

                    Thread.Sleep(5000);
                }

            } // running

            // cleanup
        }

        void SendMessage(object msg)
        {
            throw new NotImplementedException();
        }

        object GetMessage()
        {
            throw new NotImplementedException();
        }

        public UdpConnection()
        {
        }

        public bool Connect()
        {
            if (isRunning)
            {
                // already bound 
                return isRunning;
            }
            else
            {
                isRunning = true;
                int stacksize = 1024 * 128;
                worker = new Thread(Run, stacksize);
                worker.Start();
                return isRunning;
            }
        }

        public bool Create()
        {
            bool ret = true;
            client = new UdpClient(ListenPort);
            gropuEP = new IPEndPoint(IPAddress.Any, ListenPort);
            return ret;

        }

        public bool Destroy()
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            lock (locker)
            {
                if (isRunning)
                {
                    isRunning = false;
                    client.Close();
                    return isRunning;
                }
                else
                {
                    return isRunning;
                }
            }
        }

        void IConnection.SendMessage(object msg)
        {
            throw new NotImplementedException();
        }

        object IConnection.GetMessage()
        {
            throw new NotImplementedException();
        }
    }


    public class VmsClient
    {

        private IConnection connection = null;

        public VmsClient()
        {
            connection = new UdpConnection();
        }

    }

    public class VmsServer
    {
        private IConnection connection = null;

        public VmsServer()
        {

        }
    }
}

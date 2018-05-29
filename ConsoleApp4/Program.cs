using System;
using System.Collections.Generic;

namespace ConsoleApp4
{

    public class IvzLogger
    {
        public static IvzLogger Instance
        {
            get { return _sInst; }
            private set { }
        }

        public void Log(string msg)
        {
            lock (_locker)
            {
                _msgBuff.Add(msg);
            }
        }

        #region Private

        private  System.IO.StreamWriter _file = null; // new System.IO.StreamWriter(@"C:\Users\IVZ\IvzEventProxyLogging\log.txt");

        private IvzLogger()
        {

            try
            {
                _file = new System.IO.StreamWriter(@"C:\Users\IVZ\IvzEventProxyLogging\log.txt");
            }
            catch (Exception ex)
            {
                System.IO.Directory.CreateDirectory(@"C:\Users\IVZ\IvzEventProxyLogging\");
                _file = null;
            }
            finally
            {
                if (_file == null)
                    _file = new System.IO.StreamWriter(@"C:\Users\IVZ\IvzEventProxyLogging\log.txt");
            }

            _loggerThread = new System.Threading.Thread(DoWork);
            _loggerThread.Start();
            IsRunning = true; 

        }

        private static IvzLogger _sInst = new IvzLogger();

        private System.Threading.Thread _loggerThread = null;

        private bool IsRunning = false;
        private List<string> _msgBuff = new List<string>();
        private object _locker = new object();
        private static void DoWork()
        {
            List<string> dblBuff = new List<string>();
            while (System.Threading.Thread.CurrentThread.IsAlive)
            {
                lock (IvzLogger.Instance._locker)
                {
                    for (int i = 0; i < IvzLogger.Instance._msgBuff.Count; ++i)
                    {
                        dblBuff.Add(IvzLogger.Instance._msgBuff[i]);
                    }
                    IvzLogger.Instance._msgBuff.Clear();
                }

                for (int i = 0; i < dblBuff.Count; ++i)
                {
                    IvzLogger.Instance._file.WriteLine(dblBuff[i]);
                    IvzLogger.Instance._file.Flush();
                }
                dblBuff.Clear();

                // do log
                System.Threading.Thread.Sleep(200);
            }

            IvzLogger.Instance._file.Close();
        }

        #endregion

    }


    class Program
    {
        static void Main(string[] args)
        {
           while(true)
            {
                IvzLogger.Instance.Log(Console.ReadLine());
            }
        }
    }
}

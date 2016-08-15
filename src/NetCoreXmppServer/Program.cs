using NetCoreXmppServer.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreXmppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainConnectionThread mainThread = new MainConnectionThread();
            mainThread.RunThread();
        }
    }
}

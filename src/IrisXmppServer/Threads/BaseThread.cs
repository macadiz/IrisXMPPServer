using System.Threading;

namespace IrisXMPPServer.Threads
{
    public abstract class BaseThread
    {
        private Thread _thread;

        protected BaseThread() { _thread = new Thread(new ThreadStart(this.RunThread)); }

        public void Start() { _thread.Start(); }
        public void Join() { _thread.Join(); }
        public bool IsAlive { get { return _thread.IsAlive; } }
        public void Sleep(int miliseconds) { Thread.Sleep(miliseconds); }

        public abstract void RunThread();
    }
}

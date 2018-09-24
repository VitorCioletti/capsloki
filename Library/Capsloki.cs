namespace Library
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    public class Capsloki
    {
        private CancellationTokenSource _sourceToken;

        private bool _running;

        private int _intervalFrom;

        private int _intervalTo;

        /// <summary>
        /// Initializes a new instance of Capsloki
        /// </summary>
        /// <param name="intervalFrom">Set the beginning of the sleep interval</param>
        /// <param name="intervalTo">Set the end of the sleep interval</param>
        public Capsloki(int intervalFrom, int intervalTo)
        {
            _intervalFrom = intervalFrom;
            _intervalTo = intervalTo;
        }

        /// <summary>
        /// Initializes a new instance of Capsloki
        /// </summary>
        public Capsloki()
        {
            _intervalFrom = 120000;
            _intervalTo = 240000;
        }

        /// <summary>
        /// Starts your happiness and your co-workers stress :)
        /// </summary>
        public void Start()
        {
            if (_running) return;

            _sourceToken = new CancellationTokenSource();

            Task.Run(() =>
            {
                _running = true;

                Action executeHappiness = () =>
                {
                    PressCapslock();
                    Thread.Sleep(350);
                    PressCapslock();
                    Thread.Sleep(300);
                };

                for (;;)
                {
                    var begin = DateTime.Now;

                    while (DateTime.Now < begin.AddSeconds(15))
                    {
                        if (_sourceToken.IsCancellationRequested) return;
                        
                        executeHappiness();
                    }

                    Thread.Sleep(CalculateSleepTime());
                }
            });
        }

        /// <summary>
        /// Stops your happiness and your co-workers stress :(
        /// </summary>
        public void Stop()
        {
            _running = false;

            _sourceToken.Cancel();
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private void PressCapslock()
        {
            const int _KEYEVENTF_EXTENDEDKEY = 0x1;
            const int _KEYEVENTF_KEYUP = 0x2;
            const int _CAPSLOCK = 0x14;

            keybd_event(_CAPSLOCK, 0x45, _KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(_CAPSLOCK, 0x45, _KEYEVENTF_EXTENDEDKEY | _KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        private int CalculateSleepTime() => new Random().Next(_intervalFrom, _intervalTo);
    }
}

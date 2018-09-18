namespace Library
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    public class Capsloki
    {
        private const int _KEYEVENTF_EXTENDEDKEY = 0x1;

        private const int _KEYEVENTF_KEYUP = 0x2;

        private const int _CAPSLOCK = 0x14;

        private CancellationToken _token = new CancellationToken();

        /// <summary>
        /// Starts your happiness and your co-workers stress :)
        /// </summary>
        public void Start()
        {
            Task.Run(() =>
            {
                Action executeHappiness = () =>
                {
                    PressCapslock();
                    Thread.Sleep(350);
                    PressCapslock();
                    Thread.Sleep(300);
                };

                for (;;)
                {
                    if (_token.IsCancellationRequested) return;

                    var begin = DateTime.Now;

                    while (DateTime.Now < begin.AddSeconds(15))
                        executeHappiness();

                    Thread.Sleep(CalculateSleepTime());
                }
            });
        }

        /// <summary>
        /// Stops your happiness and your co-workers stress :)
        /// </summary>
        public void Stop()
        {
            var source = new CancellationTokenSource();

            _token = source.Token;

            source.Cancel();
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private void PressCapslock()
        {
            keybd_event(_CAPSLOCK, 0x45, _KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(_CAPSLOCK, 0x45, _KEYEVENTF_EXTENDEDKEY | _KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        private int CalculateSleepTime() => new Random().Next(120000, 240000);
    }
}

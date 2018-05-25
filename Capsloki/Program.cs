namespace LokiCapsLock
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    class Program
    {
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		const int KEYEVENTF_EXTENDEDKEY = 0x1;
		const int KEYEVENTF_KEYUP = 0x2;
		const int CAPSLOCK = 0x14;

		static void Main(string[] args)
		{
			// Configure your own interval between crazyness
			Func<int> calculateSleepTime = () =>
				new Random().Next(120000, 240000); // 2 to 4 minutes

            		Action turnCapsLockOn = () =>
			{
				keybd_event(CAPSLOCK, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
				keybd_event(CAPSLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
			};

			Action turnCapsLockOff = () =>
			{
				keybd_event(CAPSLOCK, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
				keybd_event(CAPSLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
			};

			Action execute = () => 
            		{ 
				Thread.Sleep(300);
				turnCapsLockOn();
				Thread.Sleep(350);
				turnCapsLockOff();
			};

		    	for (;;)
		    	{
				var begin = DateTime.Now;

				while (DateTime.Now < begin.AddSeconds(15))
			    		execute();

				Thread.Sleep(calculateSleepTime());
		    	}
		}
	}
}

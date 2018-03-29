namespace LokiCapsLock
{
	using System;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Timers;

	class Program
    {
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		const int KEYEVENTF_EXTENDEDKEY = 0x1;
		const int KEYEVENTF_KEYUP = 0x2;
		const int CAPSLOCK = 0x14;

		static void Main(string[] args)
		{
			Console.WriteLine("Loki CapsLock sucessfully turned on...");

			//// Configure your own interval between crazyness
			Func<int> calculateSleepTime = () =>
				new Random().Next(120000, 240000);

			Action turnCapsLockOn = () =>
			{
				keybd_event(CAPSLOCK, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
				keybd_event(CAPSLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
			};

			Action turnCapsLockOff = () =>
			{
				keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
				keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
			};

			Action<object, ElapsedEventArgs> execute = (o, e) =>
			{
				turnCapsLockOn();
				Console.WriteLine(e.SignalTime);
				Thread.Sleep(250);
				turnCapsLockOff();
			};

			var aTimer = new System.Timers.Timer();

			aTimer.Elapsed += new ElapsedEventHandler(execute);
			aTimer.Interval = 300;

			aTimer.Enabled = true;

			Console.ReadKey();
		}
	}
}

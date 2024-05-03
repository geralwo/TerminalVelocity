using System;
using System.Threading;
using System.Threading.Tasks;

namespace TerminalVelocity
{
    public class Input
    {
        /// <summary>
        /// The event that gets fired when a key is pressed
        /// </summary>
        public static event OnKeyPressed? KeyPressed;

        public delegate void OnKeyPressed(ConsoleKey key);

        /// <summary>
        /// The last key that was pressed.
        /// </summary>
        public static ConsoleKeyInfo LastKeyPressed;

        private static bool _isRunning;
        private static Thread? _inputThread;
        
        static Input()
        {
            StartInputListener();
        }

        /// <summary>
        /// Starts the background thread that listens for key presses.
        /// </summary>
        public static void StartInputListener()
        {
            if (_isRunning) return;

            _isRunning = true;
            _inputThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                        if (keyInfo.Key != ConsoleKey.None)
                        {
                            KeyPressed?.Invoke(keyInfo.Key);
                            LastKeyPressed = keyInfo;
                        }

                        // Flush remaining input
                        while (Console.KeyAvailable)
                            Console.ReadKey(true);
                    }

                    // A small delay to prevent high CPU usage
                    Thread.Sleep(10);
                }
            });

            _inputThread.IsBackground = true;
            _inputThread.Start();
        }

        /// <summary>
        /// Stops the background thread that listens for key presses.
        /// </summary>
        public static void StopInputListener()
        {
            _isRunning = false;
            _inputThread?.Join();
            _inputThread = null;
        }
    }
}

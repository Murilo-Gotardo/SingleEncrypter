using System.Diagnostics;

namespace SingleEncrypter.Helper
{
    internal class FinalMessageHelper
    {
        public static void Message( int op, Stopwatch _timeTaken, string? key = null)
        {
            TimeSpan _timeSpan = _timeTaken.Elapsed;

            if (op == 1)
            {
                Console.WriteLine($"""
                    ---------------
                    - File encryption succeeded
                    - Time Taken: {_timeSpan}
                    - Save your password: {key}
                    ---------------
                    """);
            }
            else if (op == 2)
            {
                Console.WriteLine($"""
                    ---------------
                    - File decryption succeeded
                    - Time Taken: {_timeSpan}
                    ---------------
                    """);
            }
        }
    }
}

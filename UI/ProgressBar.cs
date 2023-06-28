namespace SingleEncrypter.UI
{
    internal class ProgressBar
    {
        //TODO: enhance this progress bar
        public static void Update(long currentProgress, long totalProgress)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            decimal progressPercentage = (decimal)currentProgress / totalProgress *  100;

            Console.CursorLeft = 0;
            Console.Write("[");
            Console.Write(Math.Round(progressPercentage, 0) + "%");
            Console.Write("]");
        }
    }
}

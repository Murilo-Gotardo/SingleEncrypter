namespace SingleEncrypter.UI
{
    internal class Spinner
    {

        // TODO: testar este código
        private int counter = 0;

        private readonly string[] spinnerChars = { "/", "-", "\\", "|" };

        public void Start()
        {
            Console.CursorVisible = false;

            while (true)
            {
                Console.Write(spinnerChars[counter % spinnerChars.Length]);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                Thread.Sleep(100);
                counter++;
            }
        }
    }
}

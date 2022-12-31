namespace B3
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (args.Length > 0)
                {
                    string firstArgument = args[0].ToLower().Trim();
                    string secondArgument = null;

                    // Handle cases where arguments are separated by colon.
                    // Examples: /c:1234567 or /P:1234567
                    if (firstArgument.Length > 2)
                    {
                        secondArgument = firstArgument.Substring(3).Trim();
                        firstArgument = firstArgument.Substring(0, 2);
                    }
                    else if (args.Length > 1)
                        secondArgument = args[1];

                    if (firstArgument == "/c")           // Configuration mode
                    {
                        Application.Run(new B3Config());
                    }
                    else if (firstArgument == "/p")      // Preview mode
                    {
                        IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                        Application.Run(new B3Saver(previewWndHandle));
                    }
                else if (firstArgument == "/s")      // Full-screen mode
                    {
                        ShowScreenSaver();
                        Application.Run();
                    }
                    else    // Undefined argument
                    {
                        MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                            "\" is not valid.", "ScreenSaver",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else    // No arguments - treat like /c
                {
                    Application.Run(new B3Saver());
                    //ShowScreenSaver();
                    //Application.Run();
                    //// TODO
                }
            }

            static void ShowScreenSaver()
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    B3Saver screensaver = new B3Saver(screen.Bounds);
                    screensaver.Show();
                }
            }
        }
}
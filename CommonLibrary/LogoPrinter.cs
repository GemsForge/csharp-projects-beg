namespace CommonLibrary
{
    /// <summary>
    /// Provides functionality to print a logo to the console.
    /// </summary>
    public static class LogoPrinter
    {
        /// <summary>
        /// Prints a simple ASCII logo to the console.
        /// </summary>
        public static void DisplayLogo()
        {
            string asciiLogo = @"
/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
/\                                                                /\
\/    ___   ____ ___  ___ //  __       ___   ___   ____    ____   \/
/\   // \\ ||    ||\\//||    (( \     //    // \\  || \\  ||      /\
\/  (( ___ ||==  || \/ ||     \\     ((    ((   )) ||  )) ||==    \/
/\   \\_|| ||___ ||    ||    \_))     \\__  \\_//  ||_//  ||___   /\
\/                                                                \/
/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
                                                   
";
            // Output the logo to the console
            Console.WriteLine(asciiLogo);
        }
    }
}

namespace TaskTracker.BrandLogo
{
    /// <summary>
    /// Represents the brand logo for the TaskTracker application.
    /// </summary>
    public static class Logo
    {
        /// <summary>
        /// Prints the ASCII art logo of TaskTracker to the console.
        /// </summary>
        public static void DisplayLogo()
        {
            string asciiLogo = @"
  ______                   ______          _       
 / _____)                 / _____)        | |      
| /  ___  ____ ____   ___| /      ___   _ | | ____ 
| | (___)/ _  )    \ /___) |     / _ \ / || |/ _  )
| \____/( (/ /| | | |___ | \____| |_| ( (_| ( (/ / 
 \_____/ \____)_|_|_(___/ \______)___/ \____|\____)
                                                   
";
            // Output the logo to the console
            Console.WriteLine(asciiLogo);
        }
    }
}

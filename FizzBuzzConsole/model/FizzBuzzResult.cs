namespace FizzBuzzConsole.model
{
    public class FizzBuzzResult
    {
        public int Number { get; set; }
        public string Result { get; set; }  // "Fizz", "Buzz", "FizzBuzz", or the number as a string
        public DateTime PlayedAt { get; set; }  // When the user played the number
    }

}

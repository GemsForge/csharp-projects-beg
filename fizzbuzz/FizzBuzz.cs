namespace fizzbuzz
{
    public class FizzBuzz
    {
        //Point for inputted value
        public int Point { get; set; }
        //Guess representing 
        public FizzBuzzGuess Guess { get; set; }
    }

    public enum FizzBuzzGuess
    {
        NUMBER,
        FIZZ,
        BUZZ,
        FIZZBUZZ
    }
}

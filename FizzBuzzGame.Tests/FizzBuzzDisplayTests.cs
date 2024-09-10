using FizzBuzzConsole.game;

namespace FizzBuzzGame.Tests
{
    /// <summary>
    /// Unit tests for the FizzBuzzDisplay class.
    /// </summary>
    public class FizzBuzzDisplayTests
    {
        private readonly IFizzBuzzDisplay _display;

        public FizzBuzzDisplayTests()
        {
            _display = new FizzBuzzDisplay();
        }
        /// <summary>
        /// Tests that DisplayGameRules correctly outputs the game rules.
        /// </summary>
        [Fact]
        public void DisplayGameRules_Should_Output_CorrectFormat()
        {
            // Arrange
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                _display.DisplayGameRules();

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("Welcome to the FizzBuzz Game!", output);
                Assert.Contains("Here are the rules:", output);
                Assert.Contains("Let's get started!", output);
            }
            finally
            {
                //Restore the original output stream
                Console.SetOut(originalOut);
            }
        }

        /// <summary>
        /// Tests that GetValidatedInputs correctly reads and validates user input.
        /// </summary>
        [Fact]
        public void GetValidatedInputs_Should_Return_ValidInputs()
        {
            // Arrange
            var input = "10\n15\n30\n45\n50\n"; // Simulate user input
            var originalIn = Console.In;
            using var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            try
            {
                // Act
                var inputs = _display.GetValidatedInputs(5);

                // Assert
                Assert.Equal(new List<int> { 10, 15, 30, 45, 50 }, inputs);

            }
            finally
            {
                // Restore the original input stream
                Console.SetIn(originalIn);
            }
            
        }

        /// <summary>
        /// Tests that DisplayResults correctly outputs the game results.
        /// </summary>
        [Fact]
        public void DisplayResults_Should_Output_CorrectFormat()
        {
            // Arrange
            TextWriter originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            try
            {
                int totalPoints = 22;
                int fizzes = 2;
                int buzzes = 2;
                int fizzBuzzes = 1;

                // Act
                _display.DisplayResults(totalPoints, fizzes, buzzes, fizzBuzzes);

                // Assert
                var output = stringWriter.ToString();
                Assert.Contains("Total Points: 22 (Fizz 2x | Buzz 2x | FizzBuzz 1x)", output.Trim());
            }
            finally
            {
                //Restore the original output stream
                Console.SetOut(originalOut);
            }
        }
    }
}

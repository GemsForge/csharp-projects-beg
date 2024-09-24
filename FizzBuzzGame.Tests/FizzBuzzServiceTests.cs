

using FizzBuzzConsole.service;
using FizzBuzzGame;


namespace FizzBuzzGame.Tests
{
    /// <summary>
    /// Unit tests for the FizzBuzzService class.
    /// </summary>
    public class FizzBuzzServiceTests
    {
        private readonly IFizzBuzzService _fbService;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzServiceTests"/> class.
        /// </summary>
        public FizzBuzzServiceTests()
        {
            //Inject fbService interface
            _fbService = new FizzBuzzService();

        }
        /// <summary>
        /// Tests that SaveValueList correctly saves FizzBuzz guesses.
        /// </summary>
        [Fact()]
        public void SaveValueList_Should_Save_FizzBuzzValue()
        {
            //Arrange
            var values = new List<int> { 1, 3, 5, 15 };

            //Act
            _fbService.SaveValueList(values);
            var savedValues = _fbService.GetSavedValues().ToList();

            //Assert
            Assert.Equal(4, savedValues.Count); //Ensure 4 values are saved
            Assert.Equal("NUMBER", savedValues[0].Result); //1 should be "Number"
            Assert.Equal("FIZZ", savedValues[1].Result); //3 should be "Fizz"
            Assert.Equal("BUZZ", savedValues[2].Result); //5 should be "Buzz"
            Assert.Equal("FIZZBUZZ", savedValues[3].Result); //15 should be "FizzBuzz"
        }

        /// <summary>
        /// Tests that TallyPoints calculates the total points correctly.
        /// </summary>
        [Fact]
        public void TallyPoints_Should_Calculate_TotalPoints_Correctly()
        {
            // Arrange
            var values = new List<int> { 1, 3, 5, 15 }; // 1 -> 1pt, 3 -> 5pts, 5 -> 5pts, 15 -> 10pts
            _fbService.SaveValueList(values);

            // Act
            var totalPoints = _fbService.TallyPoints();

            // Assert
            Assert.Equal(21, totalPoints); // 1 + 5 + 5 + 10 = 21
        }

        /// <summary>
        /// Tests that GetSavedValues returns all saved FizzBuzz guesses.
        /// </summary>
        [Fact]
        public void GetSavedValues_Should_Return_All_Saved_Values()
        {
            // Arrange
            var values = new List<int> { 1, 2, 3 };
            _fbService.SaveValueList(values);

            // Act
            var savedValues = _fbService.GetSavedValues();

            // Assert
            Assert.NotNull(savedValues);
            Assert.Equal(3, savedValues.Count());
        }
    }
}
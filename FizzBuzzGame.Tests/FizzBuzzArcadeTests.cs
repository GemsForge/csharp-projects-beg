
using FizzBuzzConsole.game;
using FizzBuzzConsole.service;
using Moq;

namespace FizzBuzzGame.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="FizzBuzzArcade"/> class.
    /// </summary>
    public class FizzBuzzArcadeTests
    {
        private readonly Mock<IFizzBuzzService> _mockFizzBuzzService;
        private readonly Mock<IFizzBuzzDisplay> _mockFizzBuzzDisplay;
        private readonly FizzBuzzArcade _fbGame;
        private readonly string _playerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzArcadeTests"/> class.
        /// </summary>
        public FizzBuzzArcadeTests()
        {
            // Mocking dependencies: IFizzBuzzService and IFizzBuzzDisplay
            _mockFizzBuzzService = new Mock<IFizzBuzzService>();
            _mockFizzBuzzDisplay = new Mock<IFizzBuzzDisplay>();
            _playerId = Guid.NewGuid().ToString();

            // Injecting mocks into the FizzBuzzArcade instance
            _fbGame = new FizzBuzzArcade(_mockFizzBuzzService.Object, _mockFizzBuzzDisplay.Object, _playerId);
        }

        /// <summary>
        /// Tests that StartGame should save inputs and display the results.
        /// </summary>
        [Fact]
        public void StartGame_Should_SaveGamePlay_And_DisplayResults()
        {
            // Arrange
            var inputValues = new List<int> { 1, 3, 5, 15, 1 }; // Mocked input values

            // Mock the display and service behaviors
            _mockFizzBuzzDisplay.Setup(d => d.GetValidatedInputs(5)).Returns(inputValues); // User input
            _mockFizzBuzzService.Setup(s => s.CountFizzBuzzes(It.IsAny<int>())).Returns((1, 1, 1)); // Mocked counts
            _mockFizzBuzzService.Setup(s => s.TallyPoints(It.IsAny<int>())).Returns(22); // Mocked points tally

            // Act
            _fbGame.StartGame(); // Start the game, which should trigger inputs, save, and display logic

            // Assert
            _mockFizzBuzzService.Verify(s => s.SaveGamePlay(_playerId, inputValues), Times.Once); // Ensure game play is saved
            _mockFizzBuzzService.Verify(s => s.TallyPoints(It.IsAny<int>()), Times.Once); // Ensure tally is called
            _mockFizzBuzzDisplay.Verify(d => d.DisplayResults(1, 1, 1), Times.Once); // Ensure results are displayed
            _mockFizzBuzzDisplay.Verify(d => d.DisplayScore(22), Times.Once); // Ensure score is displayed
        }

        /// <summary>
        /// Tests that StartGame should reset the game state between plays.
        /// </summary>
        [Fact]
        public void StartGame_Should_ResetGameState_AfterPlay()
        {
            // Arrange
            var inputValues = new List<int> { 2, 9, 10, 30, 1 };
            _mockFizzBuzzDisplay.Setup(d => d.GetValidatedInputs(5)).Returns(inputValues);
            _mockFizzBuzzDisplay.SetupSequence(d => d.AskToPlayAgain())
                .Returns(true)  // User chooses to play again
                .Returns(false); // User chooses to stop after the second game

            // Act
            _fbGame.StartGame();

            // Assert
            _mockFizzBuzzService.Verify(s => s.ClearGamePlay(It.IsAny<int>()), Times.AtLeastOnce); // Ensure game state is reset
            _mockFizzBuzzDisplay.Verify(d => d.DisplayScore(It.IsAny<int>()), Times.AtLeastOnce); // Verify score display
        }

        /// <summary>
        /// Tests that StartGame should end the game if the user chooses not to play again.
        /// </summary>
        [Fact]
        public void StartGame_Should_End_When_UserDeclinesToPlayAgain()
        {
            // Arrange
            var inputValues = new List<int> { 1, 3, 5, 15, 7 };
            _mockFizzBuzzDisplay.Setup(d => d.GetValidatedInputs(5)).Returns(inputValues);
            _mockFizzBuzzDisplay.Setup(d => d.AskToPlayAgain()).Returns(false); // User opts out of playing again

            // Act
            _fbGame.StartGame();

            // Assert
            _mockFizzBuzzDisplay.Verify(d => d.DisplayFinalScore(It.IsAny<int>()), Times.Once); // Ensure final score is displayed
        }
    }
}

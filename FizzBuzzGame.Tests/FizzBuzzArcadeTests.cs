using FizzBuzzGame.game;
using FizzBuzzGame.model;
using FizzBuzzGame.service;
using Moq;

namespace FizzBuzzGame.Tests
{
    /// <summary>
    /// Unit test for the FizzBuzzArcade class.
    /// </summary>
    public class FizzBuzzArcadeTests
    {
        private readonly Mock<IFizzBuzzService> _mockFizzBuzzService;
        private readonly Mock<IFizzBuzzDisplay> _mockFizzBuzzDisplay;
        private readonly FizzBuzzArcade _fbGame;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzArcadeTests"/>
        /// </summary>
        public FizzBuzzArcadeTests()
        {
            // Create a mock of the IFizzBuzzService
            _mockFizzBuzzService = new Mock<IFizzBuzzService>();
            _mockFizzBuzzDisplay = new Mock<IFizzBuzzDisplay>();

            // Initialize the FizzBuzzGame with the mocked service
            _fbGame = new FizzBuzzArcade(_mockFizzBuzzService.Object, _mockFizzBuzzDisplay.Object);
        }

        /// <summary>
        /// Tests that StartGame interacts with the FizzBuzz service correctly.
        /// </summary>
        [Fact]
        public void StartGame_Should_SaveInputs_And_DisplayResults()
        {
            // Arrange
            var inputValues = new List<int> { 1, 3, 5, 15, 1 };
            var savedValues = new List<FizzBuzz>
            {
                FizzBuzz.Create(1),
                FizzBuzz.Create(3),
                FizzBuzz.Create(5),
                FizzBuzz.Create(15),
                FizzBuzz.Create(1)
            };

            _mockFizzBuzzDisplay.Setup(d => d.GetValidatedInputs(5)).Returns(inputValues);
            _mockFizzBuzzService.Setup(s => s.GetSavedValues()).Returns(savedValues);
            _mockFizzBuzzService.Setup(s => s.TallyPoints()).Returns(22);

            // Act
            _fbGame.StartGame();

            // Assert
            _mockFizzBuzzService.Verify(s => s.SaveValueList(It.IsAny<List<int>>()), Times.Once);
            _mockFizzBuzzService.Verify(s => s.TallyPoints(), Times.Once);
            _mockFizzBuzzDisplay.Verify(d => d.DisplayResults(22, 1, 1, 1), Times.Once);
        }
    }
}

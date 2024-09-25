using FizzBuzzConsole.data;
using FizzBuzzConsole.model;
using FizzBuzzConsole.service;
using Moq;

namespace FizzBuzzGame.Tests
{
    /// <summary>
    /// Unit tests for the FizzBuzzService class.
    /// </summary>
    public class FizzBuzzServiceTests
    {
        private readonly Mock<IFizzBuzzRepository> _mockRepository;
        private readonly IFizzBuzzService _fbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FizzBuzzServiceTests"/> class.
        /// </summary>
        public FizzBuzzServiceTests()
        {
            // Set up mock repository
            _mockRepository = new Mock<IFizzBuzzRepository>();
            _mockRepository.Setup(r => r.LoadResults()).Returns(new List<FizzBuzzGamePlay>());

            // Inject the mocked repository into the service
            _fbService = new FizzBuzzService(_mockRepository.Object);
        }

        /// <summary>
        /// Tests that SaveGamePlay correctly saves FizzBuzz guesses.
        /// </summary>
        [Fact]
        public void SaveGamePlay_Should_Save_FizzBuzzGameplay()
        {
            // Arrange
            var values = new List<int> { 1, 3, 5, 15 }; // Test values for FizzBuzz
            var player = "testPlayer";

            // Act
            _fbService.SaveGamePlay(player, values);

            // Assert
            _mockRepository.Verify(r => r.SaveResults(It.IsAny<List<FizzBuzzGamePlay>>()), Times.Once); // Verify the repository save method is called
        }

        /// <summary>
        /// Tests that TallyPoints calculates the total points correctly.
        /// </summary>
        [Fact]
        public void TallyPoints_Should_Calculate_TotalPoints_Correctly()
        {
            // Arrange
            var values = new List<int> { 1, 3, 5, 15 }; // 1 -> 1pt, 3 -> 5pts, 5 -> 5pts, 15 -> 10pts
            var player = "testPlayer";

            // Save the game play
            _fbService.SaveGamePlay(player, values);

            // Mock the repository to return the saved game play for TallyPoints test
            _mockRepository.Setup(r => r.LoadResults()).Returns(new List<FizzBuzzGamePlay>
            {
                new() {
                    Player = player,
                    GamePlayId = 1,
                    Guesses = new Dictionary<int, FizzBuzzGuess>
                    {
                        { 1, FizzBuzzGuess.NONE },
                        { 3, FizzBuzzGuess.FIZZ },
                        { 5, FizzBuzzGuess.BUZZ },
                        { 15, FizzBuzzGuess.FIZZBUZZ }
                    },
                    TotalPoints = 21
                }
            });

            // Act
            var totalPoints = _fbService.TallyPoints(1); // Passing the GamePlayId (1)

            // Assert
            Assert.Equal(21, totalPoints); // 1 + 5 + 5 + 10 = 21
        }

        /// <summary>
        /// Tests that GetGamePlaysForPlayer returns all saved FizzBuzz gameplays for a specific player.
        /// </summary>
        [Fact]
        public void GetGamePlaysForPlayer_Should_Return_All_GamePlays_For_Player()
        {
            // Arrange
            var player = "testPlayer";

            // Mock the repository to return a gameplay session for the testPlayer
            _mockRepository.Setup(r => r.LoadResults()).Returns(new List<FizzBuzzGamePlay>
            {
                 new() {
                    Player = player,
                    GamePlayId = 1,
                    Guesses = new Dictionary<int, FizzBuzzGuess>
                    {
                        { 1, FizzBuzzGuess.NONE },
                        { 3, FizzBuzzGuess.FIZZ },
                        { 5, FizzBuzzGuess.BUZZ },
                        { 15, FizzBuzzGuess.FIZZBUZZ }
                    },
                    TotalPoints = 21
                },
                new() {
                    Player = player,
                    GamePlayId = 2,
                    Guesses = new Dictionary<int, FizzBuzzGuess>
                    {
                        { 1, FizzBuzzGuess.NONE },
                        { 3, FizzBuzzGuess.FIZZ },
                        { 5, FizzBuzzGuess.BUZZ },
                        { 15, FizzBuzzGuess.FIZZBUZZ }
                    },
                    TotalPoints = 21
                }
            });

            // Act
            var gamePlays = _fbService.GetGamePlaysForPlayer(player);

            // Assert
            Assert.NotNull(gamePlays);
            Assert.Equal(2, gamePlays.Count()); // Ensure two game plays are returned for the player
        }
    }
}

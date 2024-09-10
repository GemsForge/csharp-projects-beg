using System.ComponentModel.DataAnnotations;

namespace FizzBuzzApi.dto
{
    /// <summary>
    /// Represents the input data for the FizzBuzz API.
    /// </summary>
    public class FizzBuzzDto
    {
        /// <summary>
        /// Gets or sets the list of integer values for FizzBuzz computation.
        /// The list must contain exactly 5 numbers.
        /// </summary>
        [Required]
        [MinLength(5, ErrorMessage = "The list must contain exactly 5 numbers.")]
        [MaxLength(5, ErrorMessage = "The list must contain exactly 5 numbers.")]
        public required List<int> Values { get; set; }
    }
}

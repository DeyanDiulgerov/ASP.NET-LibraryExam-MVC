using Library.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Library.Models
{
    public class AddBookViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Range(typeof(decimal), "0.0", "10.0", ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
    /*• Has Id – a unique integer, Primary Key
    • Has Title – a string with min length 10 and max length 50 (required)
    • Has Author – a string with min length 5 and max length 50 (required)
    • Has Description – a string with min length 5 and max length 5000 (required)
    • Has ImageUrl – a string (required)
    • Has Rating – a decimal with min value 0.00 and max value 10.00 (required)
    • Has CategoryId – an integer, foreign key (required)
    • Has Category – a Category (required)
    • Has ApplicationUsersBooks – a collection of type ApplicationUserBook*/
using System.ComponentModel.DataAnnotations;

namespace Store.DTO
{
    public class AddReviewDto
    {
        public int ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}

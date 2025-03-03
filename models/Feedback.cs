using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Products Products { get; set; }

        [StringLength(1000)]
        public string FeedbackText { get; set; }

        [Required]
        public DateTime FeedbackDate { get; set; }

        [Required]
        [RegularExpression("^[1-5]$", ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

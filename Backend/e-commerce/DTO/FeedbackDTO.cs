using System;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTO
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string username { get; set; } // Additional field to include user information
        public string FeedbackText { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Rating { get; set; }
    }
}




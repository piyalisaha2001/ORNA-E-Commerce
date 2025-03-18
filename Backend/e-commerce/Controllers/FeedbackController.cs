using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Feedback
        [HttpGet]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .ToListAsync();

            var feedbackDtos = feedbacks.Select(f => new FeedbackDto
            {
                FeedbackId = f.FeedbackId,
                UserId = f.UserId,
                ProductId = f.ProductId,
                username = f.User.username,
                FeedbackText = f.FeedbackText,
                FeedbackDate = f.FeedbackDate,
                Rating = f.Rating
            }).ToList();

            return Ok(feedbackDtos);
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);

            if (feedback == null)
            {
                return NotFound();
            }

            var feedbackDto = new FeedbackDto
            {
                FeedbackId = feedback.FeedbackId,
                UserId = feedback.UserId,
                ProductId = feedback.ProductId,
                username = feedback.User.username,
                FeedbackText = feedback.FeedbackText,
                FeedbackDate = feedback.FeedbackDate,
                Rating = feedback.Rating
            };

            return Ok(feedbackDto);
        }

        // POST: api/Feedback
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<FeedbackDto>> PostFeedback(FeedbackDto feedbackDto)
        {
            if (feedbackDto.Rating < 1 || feedbackDto.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var feedback = new Feedback
            {
                UserId = feedbackDto.UserId,
                ProductId = feedbackDto.ProductId,
                FeedbackText = feedbackDto.FeedbackText,
                FeedbackDate = DateTime.UtcNow, // Set the feedback date to the current date and time
                Rating = feedbackDto.Rating
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            feedbackDto.FeedbackId = feedback.FeedbackId;

            return CreatedAtAction(nameof(GetFeedback), new { id = feedbackDto.FeedbackId }, feedbackDto);
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.FeedbackId == id);
        }
    }
}


using FeedbackSystem.Data;
using FeedbackSystem.Models;
using FeedbackSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        // 00016599
        private readonly FeedBackSysDbContext dbContext;

        public FeedbacksController(FeedBackSysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



       


        //00016599
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetAllFeedbacks()
        {
            return await dbContext.Feedbacks
                .Include(f => f.User)
                .Select(f => new FeedbackDTO
                {
                    FeedbackId = f.FeedbackId,
                    UserId = f.UserId,
                    FeedbackTitle = f.FeedbackTitle,
                    FeedbackDescription = f.FeedbackDescription,
                    FeedbackCreatedDate = f.FeedbackCreatedDate,
                    UserName = f.User.UserName 
                })
                .ToListAsync();
        }






        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedback(int id)
        {
            var feedback = await dbContext.Feedbacks
                .Include(f => f.User)
                .Where(f => f.FeedbackId == id)
                .Select(f => new FeedbackDTO
                {
                    FeedbackId = f.FeedbackId,
                    UserId = f.UserId,
                    FeedbackTitle = f.FeedbackTitle,
                    FeedbackDescription = f.FeedbackDescription,
                    FeedbackCreatedDate = f.FeedbackCreatedDate,
                    UserName = f.User.UserName
                })
                .FirstOrDefaultAsync();

            if (feedback == null) return NotFound();

            return feedback;
        }
        [HttpPost]
        public async Task<ActionResult<FeedbackDTO>> PostFeedback(CreateFeedbackDTO feedbackDTO)
        {
            var user = await dbContext.Users.FindAsync(feedbackDTO.UserId);
            if (user == null) return BadRequest("User not found");

            var feedback = new Feedback
            {
                UserId = feedbackDTO.UserId,
                FeedbackTitle = feedbackDTO.FeedbackTitle,
                FeedbackDescription = feedbackDTO.FeedbackDescription,
                FeedbackCreatedDate = DateTime.UtcNow
            };

            dbContext.Feedbacks.Add(feedback);
            await dbContext.SaveChangesAsync();

            var feedbackDto = new FeedbackDTO
            {
                FeedbackId = feedback.FeedbackId,
                UserId = feedback.UserId,
                FeedbackTitle = feedback.FeedbackTitle,
                FeedbackDescription = feedback.FeedbackDescription,
                FeedbackCreatedDate = feedback.FeedbackCreatedDate,
                UserName = user.UserName
            };

            return CreatedAtAction(nameof(GetAllFeedbacks), new { id = feedback.FeedbackId }, feedbackDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, CreateFeedbackDTO feedbackDTO)
        {
            var feedback = await dbContext.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            var user = await dbContext.Users.FindAsync(feedbackDTO.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

        
            feedback.UserId = feedbackDTO.UserId;
            feedback.FeedbackTitle = feedbackDTO.FeedbackTitle;
            feedback.FeedbackDescription = feedbackDTO.FeedbackDescription;
            feedback.FeedbackCreatedDate = DateTime.UtcNow;

            dbContext.Entry(feedback).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); 
        }

       
        private bool FeedbackExists(int id)
        {
            return dbContext.Feedbacks.Any(f => f.FeedbackId == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await dbContext.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            dbContext.Feedbacks.Remove(feedback);
            await dbContext.SaveChangesAsync();

            return NoContent(); 
        }


    }
}

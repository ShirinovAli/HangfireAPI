using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("Hangfire API");
        //}

        [HttpPost("[action]")]
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app"));
            return Ok($"JobId: {jobId}. Welcome email send to user");
        }

        [HttpPost("[action]")]
        public IActionResult Discount()
        {
            var timeInSeconds = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Welcome to our app"), TimeSpan.FromSeconds(timeInSeconds));

            return Ok($"JobId: {jobId}. Discount email will be sent in {timeInSeconds} seconds!");
        }

        [HttpPost("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated"), Cron.Minutely);
            return Ok("Database check job initiated!");
        }

        [HttpPost("[action]")]
        public IActionResult Confirm()
        {
            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => SendWelcomeEmail("You asked to be unsubscribed!"), TimeSpan.FromSeconds(timeInSeconds));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed!"));

            return Ok("Confirmation job created");
        }
        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }
    }
}
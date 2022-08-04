using Itask5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Itask5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ConversationContext db;

        public HomeController(ILogger<HomeController> logger, ConversationContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile(string user)
        {
            ViewBag.Messages = GetMessages(user);
            ViewBag.User = user;
            return View(new ProfileViewModel());
        }

        public IQueryable<Message> GetMessages(string user)
        {
            var messages = db.Messages.Where(m => m.Reciever_Name == user);
            return messages;
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if(String.IsNullOrEmpty(user.Name))
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (!db.Users.Any(o => o.Name == user.Name))
                {
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Profile", new { user = user.Name });
            }
        }

        [Produces("application/json")]
        [HttpGet("search")]
        [Route("api/search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var users = db.Users.Where(u => u.Name.Contains(term)).Select(u => u.Name).ToList();
                return Ok(users);
            }catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpPost("messages")]
        [Route("api/messages")]
        public async Task<IActionResult> Messages([FromBody]  string username)
        {
            try
            {
                if(username != null)
                {
                    var messages = db.Messages.Where(u => u.Reciever_Name == username).ToList();
                    return Ok(messages);
                }
                else
                {
                    return BadRequest("No username");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Send(Message message)
        {
            if(db.Users.Any(u => u.Name == message.Reciever_Name))
            {
                db.Messages.Add(message);
                await db.SaveChangesAsync();
            }
            
            return RedirectToAction("Profile", new { user = message.Sender_Name });
            
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
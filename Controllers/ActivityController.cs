using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using dojo_activities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace dojo_activities.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private ActivityContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ActivityController(
            ActivityContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        [Route("home")]
        public IActionResult Index()
        {
            User CurrentUser = GetCurrentUserAsync().Result;
            List<Event> Events = _context.events.Include(user => user.User).Include(p => p.Participants).ToList();
            ViewBag.AllEvents = Events;
            ViewBag.User = CurrentUser;
            return View("Home");
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            return View("AddActivity");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Event model)
        {
            if(ModelState.IsValid)
            {
                ValidationContext val = new ValidationContext(model);
                var result = model.Validate(val);
                if(result.Count() > 0)
                {
                    TempData["error"] = result.FirstOrDefault().ErrorMessage;
                    return RedirectToAction("New");
                }
                else
                {
                    model.UserId = GetCurrentUserAsync().Result.Id;
                    _context.events.Add(model);
                    _context.SaveChanges();
                    return Redirect($"/activity/{model.EventId}");
                }
            }
            return View("AddActivity");
        }

        [HttpGet]
        [Route("activity/{EventId}")]
        public IActionResult Show(int EventId)
        {
            Event Event = _context.events.Include(user => user.User).Include(p => p.Participants).ThenInclude(u => u.User).SingleOrDefault(e => e.EventId == EventId);
            ViewBag.Event = Event;
            User CurrUser = GetCurrentUserAsync().Result;
            ViewBag.User = CurrUser;
            if(Event.Participants.Exists(u => u.UserId == CurrUser.Id))
            {
                System.Console.WriteLine("yes");
            }
            else
            {
                System.Console.WriteLine("no");
            }
            return View("Show");
        }

        [HttpGet]
        [Route("delete/{EventId}")]
        public IActionResult Delete(int EventId)
        {
            Event remove = _context.events.SingleOrDefault(e => e.EventId == EventId);
            _context.events.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("join/{EventId}")]
        public IActionResult Join(int EventId)
        {
            User CurrUser = GetCurrentUserAsync().Result;
            Event join = _context.events.SingleOrDefault(e => e.EventId == EventId);
            Participant newParticipant = new Participant
            {
                UserId = CurrUser.Id,
                EventId = join.EventId
            };
            _context.participants.Add(newParticipant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
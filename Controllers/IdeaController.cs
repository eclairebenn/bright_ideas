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
    public class IdeaController : Controller
    {
        private BeltContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public IdeaController(
            BeltContext context,
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
            List<Meet> Meets = _context.Meets.Include(user => user.User).Include(p => p.Participants).ToList();
            foreach(Meet meet in Meets)
            {
                if(meet.Date.Add(meet.Duration) < DateTime.Now)
                {
                    _context.Meets.Remove(meet);
                    _context.SaveChanges();
                }
            }
            List<Meet> AllMeets = _context.Meets.Include(user => user.User).Include(p => p.Participants).ToList(); 
            ViewBag.Register = new RegisterViewModel();
            ViewBag.Login = new LoginViewModel();
            ViewBag.AllMeets = AllMeets;
            ViewBag.User = CurrentUser;
            ViewBag.JoinError = TempData["JoinError"];
            return View("Index");
        }


        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            return View("AddActivity");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(MeetViewModel model, string DurType)
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
                    TimeSpan duration = new TimeSpan();
                    if(DurType == "day")
                    {
                        duration = new TimeSpan((int)model.Duration, 0, 0, 0);
                    }
                    else if(DurType == "hour")
                    {
                        duration = new TimeSpan(0, (int)model.Duration, 0, 0);
                    }
                    else
                    {
                        duration = new TimeSpan(0, 0, (int)model.Duration, 0);
                    }
                    DateTime dateValue = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, model.Time.Hour, model.Time.Minute, model.Time.Second);
                    Meet NewMeet = new Meet 
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Date = dateValue,
                        Duration = duration,
                        UserId = GetCurrentUserAsync().Result.Id,
                    };

                    _context.Meets.Add(NewMeet);
                    _context.SaveChanges();
                    return Redirect($"/activity/{NewMeet.MeetId}");
                }
            }
            return View("AddActivity");
        }

        [HttpGet]
        [Route("activity/{MeetId}")]
        public IActionResult Show(int MeetId)
        {
            List<Meet> Meet = _context.Meets.Include(user => user.User).Include(p => p.Participants).ThenInclude(u => u.User).Where(e => e.MeetId == MeetId).ToList();
            ViewBag.Meet = Meet;
            User CurrUser = GetCurrentUserAsync().Result;
            ViewBag.User = CurrUser;
            return View("Show");
        }

        [HttpGet]
        [Route("delete/{MeetId}")]
        public IActionResult Delete(int MeetId)
        {
            
            Meet remove = _context.Meets.SingleOrDefault(e => e.MeetId == MeetId);
            if(GetCurrentUserAsync().Result.Id != remove.UserId)
            {
                return RedirectToAction("Index");
            }
            _context.Meets.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("join/{MeetId}")]
        public IActionResult Join(int MeetId)
        {
            User CurrUser = GetCurrentUserAsync().Result;
            List<Participant> CurrPart = _context.participants.Include(m => m.Meet).Where(p => p.UserId == CurrUser.Id).ToList();
            Meet join = _context.Meets.SingleOrDefault(e => e.MeetId == MeetId);
            DateTime joinStart = join.Date;
            DateTime joinEnd = join.Date.Add(join.Duration);
            List<string> JoinError = new List<string>();
            foreach(Participant part in CurrPart)
            {
                DateTime partStart = part.Meet.Date;
                DateTime partEnd = part.Meet.Date.Add(part.Meet.Duration);
                if(joinStart >= partStart && joinStart <= partEnd)
                {
                    JoinError.Add($"There is a conflict with event {part.Meet.Title}");
                }
                else if(joinEnd >= partStart && joinEnd <= partEnd)
                {
                    JoinError.Add($"There is a conflict with event {part.Meet.Title}");
                }
            }
            if(JoinError.Count > 0)
            {
                TempData["JoinError"] = JoinError;
                return RedirectToAction("Index");
            }
            Participant newParticipant = new Participant
            {
                UserId = CurrUser.Id,
                MeetId = join.MeetId
            };
            _context.participants.Add(newParticipant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("leave/{MeetId}")]
        public IActionResult Leave(int MeetId)
        {
            User CurrUser = GetCurrentUserAsync().Result;
            Participant remove = _context.participants.SingleOrDefault(e => e.MeetId == MeetId && e.UserId == CurrUser.Id);
            _context.participants.Remove(remove);
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
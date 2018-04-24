using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using bright_ideas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace bright_ideas.Controllers
{
    [Authorize]
    public class IdeaController : Controller
    {
        private BIdeaContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public IdeaController(
            BIdeaContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        [Route("bright_ideas")]
        public IActionResult Index()
        {
            User CurrUser = GetCurrentUserAsync().Result;
            IEnumerable<Idea> AllIdeas = _context.ideas.Include(u => u.User).Include(l => l.Likes).ThenInclude(lu => lu.User);
            ViewBag.User = CurrUser;
            ViewBag.Error = TempData["idea_error"];
            List<Idea> sortIdeas = AllIdeas.OrderByDescending(i => i.Likes.Count).ToList();
            ViewBag.AllIdeas = sortIdeas;
            
            return View("Index");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddIdea(Idea model)
        {
            if(ModelState.IsValid)
            {
                model.UserId = GetCurrentUserAsync().Result.Id;
                _context.ideas.Add(model);
                _context.SaveChanges(); 
            }
            else
            {
                TempData["idea_error"] = "Idea must be a length of at least 10 characters.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("like/{IdeaId}")]
        public IActionResult Like(int IdeaId)
        {
            Like newLike = new Like
            {
                UserId = GetCurrentUserAsync().Result.Id,
                IdeaId = IdeaId,
            };

            _context.likes.Add(newLike);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("bright_ideas/{IdeaId}")]
        public IActionResult Show(int IdeaId)
        {
            Idea Idea = _context.ideas.Include(u => u.User).SingleOrDefault(i => i.IdeaId == IdeaId);
            List<Like> Likes = _context.likes.Where(i => i.IdeaId == IdeaId).Include(u => u.User).ToList();
            
            IEnumerable<Like> NonDupLikes = Likes.Distinct(new LikeComparer());
            ViewBag.Idea = Idea;
            ViewBag.Likes = NonDupLikes;
            return View();
        }

        [HttpGet]
        [Route("users/{UserId}")]
        public IActionResult DisplayUser(string UserId)
        {
            User user = _context.users.Include(i => i.Ideas).Include(l => l.Likes).SingleOrDefault(u => u.Id == UserId);
            @ViewBag.User = user;
            return View("DisplayUser");
        }

        [HttpPost]
        [Route("delete/{IdeaId}")]
        public IActionResult DeleteIdea(int IdeaId)
        {
            Idea IdeaDelete = _context.ideas.SingleOrDefault(i => i.IdeaId == IdeaId);
            if(GetCurrentUserAsync().Result.Id == IdeaDelete.UserId)
            {
                _context.ideas.Remove(IdeaDelete);
                _context.SaveChanges();
            }
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
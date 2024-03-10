using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymPlanner.Data;
using GymPlanner.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GymPlanner.Controllers
{
    [Authorize]
    public class TrainingsController : Controller
    {
        private readonly DatabaseContext _context;

        private readonly UserManager<User> _userManager;

        public TrainingsController(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Trainings
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Trainings.Include(t => t.User);
            var model = await databaseContext.ToListAsync();
            model.Sort();
            return View(model);
        }

        // GET: Trainings/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Trainings/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("id,UserId,date,durationTime")] Training training)
        {
            var user = await _userManager.GetUserAsync(User);
            training.UserId = user.Id;
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", training.UserId);
            return View(training);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trainings == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", training.UserId);
            ViewBag.userId = training.UserId;
            ViewBag.trainingId = id;
            return View(training);
        }

        // POST: Trainings/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("id,UserId,date,durationTime")] Training training)
        {
            if (id != training.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", training.UserId);
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trainings == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trainings == null)
            {
                return Problem("Entity set 'DatabaseContext.Trainings'  is null.");
            }
            var training = await _context.Trainings.FindAsync(id);
            if (training != null)
            {
                _context.Trainings.Remove(training);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return (_context.Trainings?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymPlanner.Data;
using Microsoft.AspNetCore.Authorization;

namespace GymPlanner.Controllers
{
    [Authorize]
    public class ExcercisesController : Controller
    {
        private readonly DatabaseContext _context;

        public ExcercisesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Excercises
        public async Task<IActionResult> Index(int id)
        {
            var databaseContext = _context.Excercises.Include(e => e.Training);
            var training = await _context.Trainings.FindAsync(id);
            ViewBag.trainingId = training.UserId;
            ViewBag.trainingId = id;
            return View(await databaseContext.ToListAsync());
        }

        // GET: Excercises/Create
        public IActionResult Create(int id)
        {
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "id", "id");
            ViewBag.trainingId = id;
            return View();
        }

        // POST: Excercises/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("TrainingId,name,muscleTarget")] Excercise excercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(excercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = excercise.TrainingId });
            }
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "id", "id", excercise.TrainingId);
            return View(excercise);
        }

        // GET: Excercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Excercises == null)
            {
                return NotFound();
            }

            var excercise = await _context.Excercises.FindAsync(id);
            if (excercise == null)
            {
                return NotFound();
            }
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "id", "id", excercise.TrainingId);
            ViewBag.trainingId = excercise.TrainingId;
            return View(excercise);
        }

        // POST: Excercises/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("id,TrainingId,name,muscleTarget")] Excercise excercise)
        {
            if (id != excercise.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(excercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExcerciseExists(excercise.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = excercise.TrainingId });
            }
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "id", "id", excercise.TrainingId);
            return View(excercise);
        }

        // GET: Excercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Excercises == null)
            {
                return NotFound();
            }

            var excercise = await _context.Excercises
                .Include(e => e.Training)
                .FirstOrDefaultAsync(m => m.id == id);
            if (excercise == null)
            {
                return NotFound();
            }
            ViewBag.trainingId = excercise.TrainingId;
            return View(excercise);
        }

        // POST: Excercises/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Excercises == null)
            {
                return Problem("Entity set 'DatabaseContext.Excercises'  is null.");
            }
            var excercise = await _context.Excercises.FindAsync(id);
            int trainingId = excercise.TrainingId;
            if (excercise != null)
            {
                _context.Excercises.Remove(excercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = trainingId });
        }

        private bool ExcerciseExists(int id)
        {
            return (_context.Excercises?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

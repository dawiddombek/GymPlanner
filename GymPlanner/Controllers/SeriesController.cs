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
    public class SeriesController : Controller
    {
        private readonly DatabaseContext _context;

        public SeriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Series
        public async Task<IActionResult> Index(int id)
        {
            var databaseContext = _context.Series.Include(s => s.Excercise);
            var exercise = await _context.Excercises.FindAsync(id);
            ViewBag.trainingId = exercise.TrainingId;
            ViewBag.exerciseId = id;
            return View(await databaseContext.ToListAsync());
        }

        // GET: Series/Create
        public IActionResult Create(int id)
        {
            ViewData["ExcerciseId"] = new SelectList(_context.Excercises, "id", "id");
            ViewBag.exerciseId = id;
            return View();
        }

        // POST: Series/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ExcerciseId,numberOfRepetitions,weight")] Serie serie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = serie.ExcerciseId});
            }
            ViewData["ExcerciseId"] = new SelectList(_context.Excercises, "id", "id", serie.ExcerciseId);
            return View(serie);
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            var serie = await _context.Series.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }
            ViewData["ExcerciseId"] = new SelectList(_context.Excercises, "id", "id", serie.ExcerciseId);
            ViewBag.exerciseId = serie.ExcerciseId;
            return View(serie);
        }

        // POST: Series/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("id,ExcerciseId,numberOfRepetitions,weight")] Serie serie)
        {
            if (id != serie.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerieExists(serie.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = serie.ExcerciseId });
            }
            ViewData["ExcerciseId"] = new SelectList(_context.Excercises, "id", "id", serie.ExcerciseId);
            return View(serie);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            var serie = await _context.Series
                .Include(s => s.Excercise)
                .FirstOrDefaultAsync(m => m.id == id);
            if (serie == null)
            {
                return NotFound();
            }
            ViewBag.exerciseId = serie.ExcerciseId;
            return View(serie);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Series == null)
            {
                return Problem("Entity set 'DatabaseContext.Series'  is null.");
            }
            var serie = await _context.Series.FindAsync(id);
            int exerciseId = serie.ExcerciseId;
            if (serie != null)
            {
                _context.Series.Remove(serie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = exerciseId });
        }

        private bool SerieExists(int id)
        {
            return (_context.Series?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

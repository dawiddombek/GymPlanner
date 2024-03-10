using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymPlanner.Data;
using GymPlanner.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GymPlanner
{
    [Authorize]
    public class PersonalBestsController : Controller
    {
        private readonly DatabaseContext _context;

        public PersonalBestsController(DatabaseContext context)
        {
            _context = context;
        }

        public List<(string, double)> getPersonalBestList()
        {
            var list = new List<(string, double)>();
            var endList = new List<(string, double)>();
            var tuple = ("", 0.0);
            var series = _context.Series.Where(s => s.Excercise.Training.User.UserName == User.Identity.Name);
            var exercises = _context.Excercises.Where(e => e.Training.User.UserName == User.Identity.Name);

            foreach (var serie in series)
            {
                foreach (var exercise in exercises)
                {
                    if (serie.ExcerciseId == exercise.id)
                    {
                        list.Add((exercise.name, serie.weight));
                    }
                }
            }
            foreach (var item in list)
            {
                tuple = item;
                foreach(var element in list)
                {
                    if(element.Item1 == tuple.Item1 && element.Item2 > tuple.Item2)
                    {
                        tuple.Item2 = element.Item2;
                    }
                }
                if(!endList.Contains(tuple))
                {
                    endList.Add(tuple);
                }
            }
            return endList;
        }

        // GET: PersonalBests
        public IActionResult Index()
        {
            var list = getPersonalBestList();
            return View(list);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentWProject.Models;

namespace StudentWProject.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentWProjectContext _context;

        public StudentsController(StudentWProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            string searchGrdId = Request.Query.FirstOrDefault(x => x.Key == "[0].GradeRefId").Value;

            IQueryable<Grade> gradoner = from o in _context.Grade
                                         where o.GradeId == Int32.Parse(searchGrdId)
                                         select o;

            IQueryable<Grade> allGrades = from grd in _context.Grade select grd;

            IQueryable<Student> studoner =
                  from s in _context.Student
                  from g in gradoner
                  where s.GradeRefId == g.GradeId
                  select new Student
                  {
                      Grade = g,
                      Id = s.Id,
                      Name = s.Name,
                      LastName = s.LastName
                  };

            ////Linq Join
            //IQueryable<Student> students = _context.Student
            //                .Join(_context.Grade, student => student.GradeRefId, grade => grade.GradeId,
            //                (student, grade) => new Student
            //                {
            //                    Id = student.Id,
            //                    Name = student.Name,
            //                    LastName = student.LastName,
            //                    GradeRefId = grade.GradeId,
            //                    Grade = new Grade { GradeId = grade.GradeId, GradeName = grade.GradeName }
            //                });

            //join
            IQueryable<Student> studs =
                  from s in _context.Student
                  from g in _context.Grade
                  from a in _context.Ambion
                  where s.AmbionRefId==a.AmbionId
                  where s.GradeRefId == g.GradeId
                  select new Student
                  {
                      Grade = g,
                      Ambion=a,
                      Id = s.Id,
                      Name = s.Name,
                      LastName = s.LastName
                  };

            ViewData["selectlist"] = new SelectList(await allGrades.Distinct().ToListAsync(), "GradeId", "GradeName");
            IQueryable<Student> ss;
            if (searchGrdId == null)
            {
                ss = studs;
            }
            else
            {
                ss = studoner;
            }

            return View(await ss.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //select
            IQueryable<Grade> GradesQuery = from m in _context.Grade
                                            orderby m.GradeId
                                            select m;
            IQueryable<Ambion> AmbionsQuery = from n in _context.Ambion
                                            orderby n.AmbionId
                                            select n;
            //linq select
            IQueryable<Grade> grd = _context.Grade.Select(g => new Grade { GradeId = g.GradeId });
            //linq select to viewmodel
            IQueryable<ExampleViewModel> grd1 = _context.Grade.Select(
               g => new ExampleViewModel { Name = g.GradeName }
               );
            ViewData["selectlist"] = new SelectList(await GradesQuery.Distinct().ToListAsync(), "GradeId", "GradeName");
            ViewData["selectlist"] = new SelectList(await AmbionsQuery.Distinct().ToListAsync(), "AmbionId", "AmbionName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IQueryable<object> GradesQuery = from m in _context.Grade
                                             orderby m.GradeId
                                             select m;
            IQueryable<object> AmbionsQuery = from n in _context.Ambion
                                             orderby n.AmbionId
                                             select n;
            ViewData["selectlist2"] = new SelectList(await GradesQuery.Distinct().ToListAsync(), "GradeId", "GradeName");
            ViewData["selectlist2"] = new SelectList(await AmbionsQuery.Distinct().ToListAsync(), "AmbionId", "AmbionName");

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentName,CurrentGradeID")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["GradeRefId"] = new SelectList(_context.Set<Grade>(), "GradeId", "GradeId", student.GradeRefId);
            ViewData["AmbionRefId"] = new SelectList(_context.Set<Ambion>(), "AmbionId", "AmbionId", student.AmbionRefId);
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
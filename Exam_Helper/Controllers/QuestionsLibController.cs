﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Exam_Helper.Controllers
{
    public class QuestionsLibController : Controller
    {
        private readonly CommonDbContext _context;

        public QuestionsLibController(CommonDbContext context)
        {
            _context = context;
        }

        
        // GET: QuestionsLib
        public async Task<IActionResult> Index(string SearchString)
        {
            
            var ques = from que in _context.Question
                       select que; //await _context.Question.ToListAsync();
            if (!string.IsNullOrEmpty(SearchString))
                ques = ques.Where(x => x.Title.Contains(SearchString)
                                 || x.Proof.Contains(SearchString)
                                 || x.TagIds.Contains(SearchString)
                                 || x.Definition.Contains(SearchString));
            
            return View(await ques.ToListAsync());
        }

       

        // GET: QuestionsLib/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: QuestionsLib/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuestionsLib/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Definition,Title,Proof,TagIds")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.CreationDate = DateTime.Now;
                question.UpdateDate = DateTime.Now;
                question.Author = "Admin";
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: QuestionsLib/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: QuestionsLib/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  [Bind("Id,Definition,Title,Proof,Author,TagIds")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var old = await _context.Question.AsNoTracking().FirstAsync(x => x.Id == id);
                    question.CreationDate = old.CreationDate;
                    question.UpdateDate = DateTime.Now;
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
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
            return View(question);
        }

        // GET: QuestionsLib/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: QuestionsLib/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
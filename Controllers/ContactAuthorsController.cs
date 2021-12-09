using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5_1_.Models;

namespace Lab5_1_.Controllers
{
    public class ContactAuthorsController : Controller
    {
        private readonly BookContext _context;

        public ContactAuthorsController(BookContext context)
        {
            _context = context;
        }

        // GET: ContactAuthors
        public async Task<IActionResult> Index()
        {
            var bookContext = _context.ContactAuthors.Include(c => c.Author);
            return View(await bookContext.ToListAsync());
        }

        // GET: ContactAuthors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactAuthor = await _context.ContactAuthors
                .Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.ContactAuthorId == id);
            if (contactAuthor == null)
            {
                return NotFound();
            }

            return View(contactAuthor);
        }

        // GET: ContactAuthors/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId");
            return View();
        }

        // POST: ContactAuthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactAuthorId,phoneNumber,city,address,AuthorId")] ContactAuthor contactAuthor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactAuthor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", contactAuthor.AuthorId);
            return View(contactAuthor);
        }

        // GET: ContactAuthors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactAuthor = await _context.ContactAuthors.FindAsync(id);
            if (contactAuthor == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", contactAuthor.AuthorId);
            return View(contactAuthor);
        }

        // POST: ContactAuthors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactAuthorId,phoneNumber,city,address,AuthorId")] ContactAuthor contactAuthor)
        {
            if (id != contactAuthor.ContactAuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactAuthor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactAuthorExists(contactAuthor.ContactAuthorId))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", contactAuthor.AuthorId);
            return View(contactAuthor);
        }

        // GET: ContactAuthors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactAuthor = await _context.ContactAuthors
                .Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.ContactAuthorId == id);
            if (contactAuthor == null)
            {
                return NotFound();
            }

            return View(contactAuthor);
        }

        // POST: ContactAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactAuthor = await _context.ContactAuthors.FindAsync(id);
            _context.ContactAuthors.Remove(contactAuthor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactAuthorExists(int id)
        {
            return _context.ContactAuthors.Any(e => e.ContactAuthorId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatabaseAccess.Access;
using DatabaseAccess.Database;
using Microsoft.AspNetCore.Http;
using LibraryTask.Models;
using System.Diagnostics;

namespace LibraryTask.Controllers
{
	public class BooksController : Controller
	{
		private readonly LibraryContext libraryContext;
		[TempData]
		public string Message { get; set; }

		public BooksController(LibraryContext context)
		{
			libraryContext = context;
		}

		// GET: Books
		public async Task<IActionResult> List(string? message, bool? success)
		{
			if (message != null)
				ViewBag.Message = message;

			if (success != null)
				ViewBag.Success = success;

			return View(await libraryContext.Books.ToListAsync());
		}

		// GET: Books/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var book = await libraryContext.Books.FirstOrDefaultAsync(m => m.BookID == id);
			if (book == null)
			{
				return NotFound();
			}

			return View(book);
		}

		public IActionResult BookIt(int id)
		{
			Book book = libraryContext.Books.FirstOrDefault(b => b.BookID == id);
			User user = libraryContext.Users.FirstOrDefault(u => u.Login.Equals(User.Identity.Name));

			if (book == null || user == null)
			{
				return NotFound();
			}

			Reservation reservation = libraryContext.Reservations.FirstOrDefault(r => r.Book == book && r.User == user);
			if (reservation != null)
			{
				return RedirectToAction("List", "Books", new { message = "You already reserved book: " + book.Title + ".", success = false });
			}
			else
			{
				reservation = new Reservation
				{
					ReservationDate = DateTime.UtcNow,
					User = user,
					Book = book
				};

				libraryContext.Add(reservation);
				libraryContext.SaveChanges();
			}
			return RedirectToAction("List", "Books", new { message = "Successfully booked: " + book.Title + ".", success = true });
		}

		// GET: Books/Details/5
		public async Task<IActionResult> Reservations(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Book book = await libraryContext.Books
				.Include(x => x.Reservations)
				.ThenInclude(x => x.User)
				.SingleAsync(x => x.BookID == id);

			if (book == null)
			{
				return NotFound();
			}

			BookReservationsModel bookReservationsModel = new BookReservationsModel
			{
				BookTitle = book.Title,
				Reservations = book.Reservations.OrderBy(r => r.ReservationDate).ToList()
			};

			return View(bookReservationsModel);
		}

		// GET: Books/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Books/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("BookID,Title,Author,PublishDate,Description")] Book book)
		{
			if (ModelState.IsValid)
			{
				libraryContext.Add(book);
				await libraryContext.SaveChangesAsync();
				return RedirectToAction("List", "Books");
			}
			return View(book);
		}

		// GET: Books/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var book = await libraryContext.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}
			return View(book);
		}

		// POST: Books/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Author,PublishDate,Description")] Book book)
		{
			if (id != book.BookID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					libraryContext.Update(book);
					await libraryContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BookExists(book.BookID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("List", "Books");
			}
			return View(book);
		}

		// GET: Books/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var book = await libraryContext.Books.FirstOrDefaultAsync(m => m.BookID == id);
			if (book == null)
			{
				return NotFound();
			}

			return View(book);
		}

		// POST: Books/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			Book book = await libraryContext.Books
				.Include(x => x.Reservations)
				.ThenInclude(x => x.User)
				.SingleAsync(x => x.BookID == id);

			if (book == null)
			{
				return NotFound();
			}

			foreach (var reservation in book.Reservations)
			{
				libraryContext.Reservations.Remove(reservation);
			}

			libraryContext.Books.Remove(book);



			await libraryContext.SaveChangesAsync();

			return RedirectToAction("List", "Books");
		}

		private bool BookExists(int id)
		{
			return libraryContext.Books.Any(e => e.BookID == id);
		}
	}
}

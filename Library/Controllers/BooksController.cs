using Library.Contracts;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService bookService;

        public BooksController
            (IBookService _bookService)
        {
            this.bookService = _bookService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await bookService.GetAllBooksAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddBookViewModel()
            {
                Categories = await bookService.GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await bookService.CreateBookAsync(model);

                return RedirectToAction("All", "Books");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not create book");

                return View(model);
            }
        }

        public async Task<IActionResult> Mine()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var model = await bookService.ShowMyBooksAsync(userId);

            return View("Mine", model);
        }

        public async Task<IActionResult> AddToCollection(int bookId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                await bookService.AddBookToMyCollectionAsync(bookId, userId);
            }
            catch (Exception)
            {
                throw new ArgumentException("Could not add book to your collection");
            }

            return RedirectToAction("All", "Books");
        }

        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                await bookService.RemoveBookFromMyCollectionAsync(bookId, userId);
            }
            catch (Exception)
            {
                throw new ArgumentException("Could not remove book from your collection");
            }

            return RedirectToAction("Mine", "Books");
        }
    }
}

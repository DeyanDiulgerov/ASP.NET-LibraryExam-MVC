using Library.Contracts;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService
            (LibraryDbContext _context)
        {
            context = _context;
        }

        public async Task CreateBookAsync(AddBookViewModel model)
        {
            var book = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                CategoryId = model.CategoryId          
            };

            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BooksListViewModel>> GetAllBooksAsync()
        {
            var books = await context.Books
                .Include(b => b.Category)
                .ToListAsync();

            return books
                .Select(b => new BooksListViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Category = b?.Category?.Name,
                    Rating = b.Rating,
                    ImageUrl = b.ImageUrl,
                    Author = b.Author
                });
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task AddBookToMyCollectionAsync(int bookId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();

            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            if(user == null)
            {
                throw new ArgumentException("Could not find that user");
            }

            if(book == null)
            {
                throw new ArgumentException("Could not find that user");
            }

            if(!user.ApplicationUsersBooks.Any(b => b.BookId == bookId))
            {
                user.ApplicationUsersBooks.Add(new ApplicationUserBook()
                {
                    Book = book,
                    BookId = book.Id,
                    ApplicationUser = user,
                    ApplicationUserId = user.Id
                });

                await context.SaveChangesAsync();
            }

        }

        public async Task RemoveBookFromMyCollectionAsync(int bookId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();


            if (user == null)
            {
                throw new ArgumentException("Could not find that user");
            }

            var book = user.ApplicationUsersBooks.FirstOrDefault(b => b.BookId == bookId);

            if(book != null)
            {
                user.ApplicationUsersBooks.Remove(book);

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MyCollectionBooksViewModel>> ShowMyBooksAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(ub => ub.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Could not find that user");
            }

            return user.ApplicationUsersBooks
                .Select(b => new MyCollectionBooksViewModel()
                {
                    Id = b.BookId,
                    ImageUrl = b.Book.ImageUrl,
                    Title = b.Book.Title,
                    Author = b.Book.Author,
                    Category = b.Book.Category?.Name,
                    Description = b.Book.Description
                });
        }
    }
}

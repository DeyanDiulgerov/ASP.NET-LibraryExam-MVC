using Library.Data.Models;
using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task CreateBookAsync(AddBookViewModel model);

        Task<IEnumerable<BooksListViewModel>> GetAllBooksAsync();

        Task<IEnumerable<Category>> GetCategories();


        Task AddBookToMyCollectionAsync(int bookId, string userId);

        Task RemoveBookFromMyCollectionAsync(int bookId, string userId);

        Task<IEnumerable<MyCollectionBooksViewModel>> ShowMyBooksAsync(string userId);
    }
}

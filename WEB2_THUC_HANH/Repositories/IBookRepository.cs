using WEB2_THUC_HANH.Model.Domain;
using WEB2_THUC_HANH.Model.DTO;

namespace WEB2_THUC_HANH.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks();

        BookWithAuthorAndPublisherDTO GetBookById(int id);

        addBookRequestDTO AddBook(addBookRequestDTO addBookRequestDTO);

        addBookRequestDTO? UpdateBookById(int id, addBookRequestDTO bookDTO);

        Book? DeleteBookById(int id);
    }
}
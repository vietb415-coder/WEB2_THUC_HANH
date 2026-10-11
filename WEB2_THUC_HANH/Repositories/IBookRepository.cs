using Microsoft.AspNetCore.Mvc;
using WEB2_THUC_HANH.Model.Domain;
using WEB2_THUC_HANH.Model.DTO;

namespace WEB2_THUC_HANH.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks([FromQuery] string? filterOn,
    [FromQuery] string? filterQuery,
    [FromQuery] string? sortBy,
    [FromQuery] bool isAscending,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 100);

        BookWithAuthorAndPublisherDTO GetBookById(int id);

        addBookRequestDTO AddBook(addBookRequestDTO addBookRequestDTO);

        addBookRequestDTO? UpdateBookById(int id, addBookRequestDTO bookDTO);

        Book? DeleteBookById(int id);
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB2_THUC_HANH.Model.Domain;
using WEB2_THUC_HANH.Repositories;
using WEB2_THUC_HANH.Data;
using WEB2_THUC_HANH.Model.DTO;
using WebAPI_simple.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;


namespace WEB2_THUC_HANH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;

        public BooksController(AppDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
        }

        [HttpGet("get-all-books")]
        [Authorize(Roles = "Read")]
        public IActionResult GetAll(
    [FromQuery] string? filterOn,
    [FromQuery] string? filterQuery,
    [FromQuery] string? sortBy,
    [FromQuery] bool isAscending,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 100)
        {
            // su dung reposity pattern
            var allBooks = _bookRepository.GetAllBooks(
                filterOn,
                filterQuery,
                sortBy,
                isAscending,
                pageNumber,
                pageSize);

            return Ok(allBooks);
        }
        [HttpGet]
        [Route("get-book-by-id/{id}")]
        [Authorize(Roles = "Read")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            return Ok(bookWithIdDTO);
        }
        [HttpPost("add-book")]
        [Authorize(Roles = "Write")]
        [ValidateModel]
        //[Authorize(Roles = "Write")]
        public IActionResult AddBook([FromBody] addBookRequestDTO addBookRequestDTO)
        {
            if (ValidateAddBook(addBookRequestDTO))
            {
                var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
                return Ok(bookAdd);
            }

            return BadRequest(ModelState);
        }

        private bool ValidateAddBook(addBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(
                    nameof(addBookRequestDTO),
                    "Please add book data"
                );

                return false;
            }

            // kiem tra Description NotNull
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(
                    nameof(addBookRequestDTO.Description),
                    $"{nameof(addBookRequestDTO.Description)} cannot be null"
                );
            }

            // kiem tra rating (0,5)
            if (addBookRequestDTO.Rate < 0 ||
                addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(
                    nameof(addBookRequestDTO.Rate),
                    $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5"
                );
            }

            // kiem tra PublisherID co ton tai khong
            var publisherExists = _dbContext.Publishers.Any(
                x => x.Id == addBookRequestDTO.PublisherID);

            if (publisherExists == false)
            {
                ModelState.AddModelError(
                    nameof(addBookRequestDTO.PublisherID),
                    $"PublisherID {addBookRequestDTO.PublisherID} does not exist"
                );
            }

            // kiem tra Book phai co Author
            if (addBookRequestDTO.AuthorIds == null ||
                addBookRequestDTO.AuthorIds.Count == 0)
            {
                ModelState.AddModelError(
                    nameof(addBookRequestDTO.AuthorIds),
                    "Book must have at least one Author"
                );
            }
            else
            {
                // kiem tra tung AuthorID co ton tai khong
                foreach (var authorId in addBookRequestDTO.AuthorIds)
                {
                    var authorExists = _dbContext.Authors.Any(
                        x => x.Id == authorId);

                    if (authorExists == false)
                    {
                        ModelState.AddModelError(
                            nameof(addBookRequestDTO.AuthorIds),
                            $"AuthorID {authorId} does not exist"
                        );
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        [HttpPut("update-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult UpdateBookById(
    [FromRoute] int id,
    [FromBody] addBookRequestDTO bookDTO)
        {
            var updatedBook = _bookRepository.UpdateBookById(id, bookDTO);

            if (updatedBook == null)
            {
                return NotFound();
            }

            return Ok(updatedBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult DeleteBookById([FromRoute] int id)
        {
            var deletedBook = _bookRepository.DeleteBookById(id);

            if (deletedBook == null)
            {
                return NotFound();
            }

            return Ok(deletedBook);
        }   


    }
}
    
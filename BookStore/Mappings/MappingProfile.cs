using AutoMapper;
using BookStore.Application.AuthorOperations.Command;
using BookStore.Application.AuthorOperations.Command.CreateAuthor;
using BookStore.Application.AuthorOperations.Query;
using BookStore.Application.BookOperations.Command.CreateBook;
using BookStore.Application.BookOperations.Command.UpdateBook;
using BookStore.Application.BookOperations.Query.GetBookDetail;
using BookStore.Application.BookOperations.Query.GetBooks;
using BookStore.Application.GenreOperations.Command;
using BookStore.Application.GenreOperations.Query;
using BookStore.Models;

namespace BookStore.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map CreateBookModel to Book
        CreateMap<CreateBookModel, Book>();
        CreateMap<UpdateBookModel, Book>();
        CreateMap<Book, BookDetailViewModel>()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToString("dd/MM/yyyy")));

        // Map Book to BooksViewModel
        CreateMap<Book, BooksViewModel>()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToString("dd/MM/yyyy")));

        CreateMap<CreateGenreModel, Genre>();
        CreateMap<UpdateGenreModel, Genre>();
        CreateMap<Genre, GenresViewModel>();

        CreateMap<Genre, GenreDetailViewModel>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books.Select(book => new BookDetailViewModel
            {
                Title = book.Title,
                Genre = book.Genre.Name,
                PublishedDate = book.PublishedDate.ToString("dd/MM/yyyy"),
                Author = book.Author.FirstName + " " + book.Author.LastName,
                ISBN = book.ISBN,
                Price = book.Price
            })));

        CreateMap<CreateAuthorModel, Author>();
        CreateMap<UpdateAuthorModel, Author>();
        CreateMap<Author, AuthorsViewModel>();
        CreateMap<Author, AuthorDetailViewModel>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books.Select(book => new BookDetailViewModel
            {
                Title = book.Title,
                Genre = book.Genre.Name,
                PublishedDate = book.PublishedDate.ToString("dd/MM/yyyy"),
                Author = book.Author.FirstName + " " + book.Author.LastName,
                ISBN = book.ISBN,
                Price = book.Price
            })));
    }
}
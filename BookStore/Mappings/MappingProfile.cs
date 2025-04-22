using AutoMapper;
using BookStore.BookOperations;
using BookStore.Common;
using BookStore.Models;

namespace BookStore.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map CreateBookModel to Book
        CreateMap<CreateBookModel, Book>();

        // Map Book to BookDetailViewModel
        CreateMap<Book, BookDetailViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()))
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToString("dd/MM/yyyy")));

        // Map Book to BooksViewModel
        CreateMap<Book, BooksViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()))
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToString("dd/MM/yyyy")));
    }
}
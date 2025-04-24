using BookStore.Application.BookOperations.Command.CreateBook;
using TestSetup;
using FluentAssertions;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Command.CreateBook
{
    public class CreateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("Lord of the Rings", 0, 0,0)]
        [InlineData("Lord of the Rings", 0, 1,1)]
        [InlineData("", 0, 0,2)]
        [InlineData("", 100, 10,3)]
        [InlineData("Lor", 0, 10,0)]
        [InlineData("Lor", 100, 0,0)]
        [InlineData("Lord", 0, 0,1)]
        [InlineData("Lord", 0, 10,0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string title, int genreId,int price,int authorId)
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = title,
                GenreId = genreId,
                PublishedDate = DateTime.Now.Date.AddYears(-1),
                Price = price,
                ISBN = "1234567890",
                AuthorId = authorId,
            };

            //act
            CreateBookModelValidator validator = new CreateBookModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldReturnError()
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = "Lord Of The Rings",
                GenreId = 1,
                Price = 10,
                PublishedDate = DateTime.Now.Date,
                ISBN = "1234567890",
                AuthorId = 1,
            };

            //act
            CreateBookModelValidator validator = new CreateBookModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

         [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnErrors()
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = "Lord Of The Rings",
                GenreId = 1,
                Price = 10,
                PublishedDate = DateTime.Now.Date.AddYears(-2),
                ISBN = "1234567890",
                AuthorId = 1,
            };

            //act
            CreateBookModelValidator validator = new CreateBookModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
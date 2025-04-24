using BookStore.Application.BookOperations.Command.UpdateBook;
using TestSetup;
using FluentAssertions;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Command.UpdateBook
{
    public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
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
            UpdateBookCommand command = new UpdateBookCommand(null, null);
            command.UpdatedBook = new UpdateBookModel()
            {
                Title = title,
                GenreId = genreId,
                Price = price,
                AuthorId = authorId,
            };

            //act
            UpdateBookModelValidator validator = new UpdateBookModelValidator();
            var result = validator.Validate(command.UpdatedBook);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

         [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnErrors()
        {
            //arrange
            UpdateBookCommand command = new UpdateBookCommand(null, null);
            command.UpdatedBook = new UpdateBookModel()
            {
                Title = "Lord Of The Rings",
                GenreId = 1,
                Price = 10,
                AuthorId = 1,
            };

            //act
            UpdateBookModelValidator validator = new UpdateBookModelValidator();
            var result = validator.Validate(command.UpdatedBook);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
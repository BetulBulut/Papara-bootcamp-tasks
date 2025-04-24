using TestSetup;
using FluentAssertions;
using BookStore.Application.GenreOperations.Command;

namespace Tests.BookStore.UnitTests.Application.GenreOperations.Command.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        
        [Theory]
        [InlineData("science", "")]
        [InlineData("", "description")] 
        [InlineData("science", "de")] 
        [InlineData("s", "d")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string name, string description)
        {
            //arrange
            UpdateGenreCommand command = new UpdateGenreCommand(null, null);
            command.UpdatedGenre = new UpdateGenreModel()
            {
                Name = name,
                Description = description
            };

            //act
            UpdateGenreModelValidator validator = new UpdateGenreModelValidator();
            var result = validator.Validate(command.UpdatedGenre);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnErrors()
        {
            //arrange
            UpdateGenreCommand command = new UpdateGenreCommand(null, null);
            command.UpdatedGenre = new UpdateGenreModel()
            {
                Name = "science",
                Description = "description"
            };

            //act
            UpdateGenreModelValidator validator = new UpdateGenreModelValidator();
            var result = validator.Validate(command.UpdatedGenre);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
using BookStore.Application.GenreOperations.Command;
using TestSetup;
using FluentAssertions;

namespace Tests.GenreStore.UnitTests.Application.GenreOperations.Command.CreateGenre
{
    public class CreateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("science", "")]
        [InlineData("", "description")] 
        [InlineData("science", "de")] 
        [InlineData("s", "d")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string name, string description)
        {
            //arrange
            CreateGenreCommand command = new CreateGenreCommand(null, null);
            command.Model = new CreateGenreModel()
            {
                Name = name,
                Description = description
            };

            //act
            CreateGenreModelValidator validator = new CreateGenreModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

         [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnErrors()
        {
            //arrange
            CreateGenreCommand command = new CreateGenreCommand(null, null);
            command.Model = new CreateGenreModel()
            {
                Name = "science",
                Description = "description"
            };

            //act
            CreateGenreModelValidator validator = new CreateGenreModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
using TestSetup;
using FluentAssertions;
using BookStore.Application.AuthorOperations.Command;

namespace Tests.BookStore.UnitTests.Application.AuthorOperations.Command.UpdateAuthor
{
    public class UpdateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        
        [Theory]
        [InlineData("fi", "")]
        [InlineData("", "la")] 
        [InlineData("firs", "l")] 
        [InlineData("f", "l")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string FirstName, string LastName)
        {
            //arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            command.UpdatedAuthor = new UpdateAuthorModel()
            {
                FirstName = FirstName,
                LastName = LastName
            };

            //act
            UpdateAuthorModelValidator validator = new UpdateAuthorModelValidator();
            var result = validator.Validate(command.UpdatedAuthor);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnErrors()
        {
            //arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            command.UpdatedAuthor = new UpdateAuthorModel()
            {
                FirstName = "name",
                LastName = "Lastname"
            };

            //act
            UpdateAuthorModelValidator validator = new UpdateAuthorModelValidator();
            var result = validator.Validate(command.UpdatedAuthor);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
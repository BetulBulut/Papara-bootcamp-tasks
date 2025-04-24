using TestSetup;
using FluentAssertions;
using BookStore.Application.AuthorOperations.Command.CreateAuthor;

namespace Tests.AuthorStore.UnitTests.Application.AuthorOperations.Command.CreateAuthor
{
    public class CreateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("name", "")]
        [InlineData("", "name")] 
        [InlineData("n", "n")] 
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string firstName, string lastName)
        {
            //arrange
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = new DateTime(1990, 1, 1)
            };

            //act
            CreateAuthorModelValidator validator = new CreateAuthorModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }
        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldReturnError()
        {
            //arrange
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel()
            {
                FirstName = "name",
                LastName = "name",
                BirthDate = DateTime.Now.Date
            };

        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnErrors()
        {
            //arrange
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel()
            {
                FirstName = "name",
                LastName = "name",
                BirthDate = new DateTime(1990, 1, 1)
            };

            //act
            CreateAuthorModelValidator validator = new CreateAuthorModelValidator();
            var result = validator.Validate(command.Model);

            //assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
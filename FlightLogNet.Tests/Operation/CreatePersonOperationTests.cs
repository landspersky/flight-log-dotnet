namespace FlightLogNet.Tests.Operation
{
    using FlightLogNet.Operation;
    using FlightLogNet.Repositories.Interfaces;
    using Integration;
    using Models;
    using Moq;
    using Xunit;

    public class CreatePersonOperationTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IPersonRepository> mockPersonRepository;
        private readonly Mock<IClubUserDatabase> mockClubUserDatabase;

        public CreatePersonOperationTests()
        {
            this.mockRepository = new(MockBehavior.Strict);

            this.mockPersonRepository = this.mockRepository.Create<IPersonRepository>();
            this.mockClubUserDatabase = this.mockRepository.Create<IClubUserDatabase>();
        }

        private CreatePersonOperation CreateCreatePersonOperation()
        {
            return new(
                this.mockPersonRepository.Object,
                this.mockClubUserDatabase.Object);
        }

        [Fact]
        public void Execute_ShouldReturnNull()
        {
            // Arrange
            var createPersonOperation = this.CreateCreatePersonOperation();

            // Act
            var result = createPersonOperation.Execute(null);

            // Assert
            Assert.Null(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Execute_ShouldCreateGuest()
        {
            // Arrange
            var createPersonOperation = this.CreateCreatePersonOperation();
            PersonModel personModel = new PersonModel
            {
                MemberId = 0,
                Address = new() { City = "NY", PostalCode = "456", Street = "2nd Ev", Country = "USA" },
                FirstName = "John",
                LastName = "Smith"
            };
            this.mockPersonRepository.Setup(repository => repository.AddGuestPerson(personModel)).Returns(10);

            // Act
            var result = createPersonOperation.Execute(personModel);

            // Assert
            Assert.True(result > 0);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Execute_ShouldReturnExistingClubMember()
        {
            // Arrange
            var createPersonOperation = this.CreateCreatePersonOperation();
            PersonModel personModel = new PersonModel
            {
                FirstName = "Jan",
                LastName = "Novák",
                MemberId = 3
            };
            long id = 333;
            this.mockPersonRepository.Setup(repository => repository.TryGetPerson(personModel, out id)).Returns(true);

            // Act
            var result = createPersonOperation.Execute(personModel);

            // Assert
            Assert.Equal(id, result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Execute_ShouldCreateNewClubMember()
        {
            // Arrange
            // TODO 7.1: Naimplementujte test s použitím mockù
            var createPersonOperation = this.CreateCreatePersonOperation();
            var personModel = new PersonModel
            {
                FirstName = "Jan",
                LastName = "Novák",
                MemberId = 3
            };
            long defaultId = 0;
            this.mockPersonRepository.Setup(repository => repository.TryGetPerson(personModel, out defaultId)).Returns(false);
            long id = 333;
            this.mockClubUserDatabase.Setup(db => db.TryGetClubUser(personModel.MemberId, out personModel)).Returns(true);
            this.mockPersonRepository.Setup(repository => repository.CreateClubMember(personModel)).Returns(id);

            // Act
            var result = createPersonOperation.Execute(personModel);

            // Assert
            Assert.Equal(id, result);
            this.mockRepository.VerifyAll();
        }
    }
}

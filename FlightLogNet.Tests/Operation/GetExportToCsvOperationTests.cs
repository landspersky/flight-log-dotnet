namespace FlightLogNet.Tests.Operation
{
    using FlightLogNet.Models;
    using System;
    using FlightLogNet.Operation;

    using Xunit;
    using Moq;
    using FlightLogNet.Repositories.Interfaces;
    using System.Text;
    using System.IO;

    //GetExportToCsvOperation getExportToCsvOperation
    public class GetExportToCsvOperationTests
    {
        private readonly Mock<IFlightRepository> mockFlightRepository;
        private readonly GetExportToCsvOperation getExportToCsvOperation;
        private readonly MockRepository mockRepository;

        public GetExportToCsvOperationTests()
        {
            this.mockRepository = new(MockBehavior.Strict);
            this.mockFlightRepository = mockRepository.Create<IFlightRepository>();
            this.getExportToCsvOperation = new GetExportToCsvOperation(this.mockFlightRepository.Object);
        }

        [Fact]
        public void Execute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            var towplane = new FlightModel
            {
                TakeoffTime = new DateTime(2025, 5, 6, 8, 0, 0, DateTimeKind.Utc),
                LandingTime = new DateTime(2025, 5, 6, 8, 30, 0, DateTimeKind.Utc),
                Airplane = new AirplaneModel() { Id = 1, Immatriculation = "ABC123", Type = "L-13A Blaník", },
                Pilot = new PersonModel() { MemberId = 1, LastName = "Nejezchleba" }
            };

            var glider = new FlightModel
            {
                TakeoffTime = new DateTime(2025, 5, 6, 8, 5, 0, DateTimeKind.Utc),
                LandingTime = new DateTime(2025, 5, 6, 8, 45, 0, DateTimeKind.Utc),
                Airplane = new AirplaneModel() { Id = 2, Immatriculation = "XYZ789", Type = "Zlín Z-42M" },
                Pilot = new PersonModel() { MemberId = 2, LastName = "Brzobohatý" }
            };

            mockFlightRepository.Setup(repository => repository.GetReport()).Returns(
            [
                new ReportModel()
            {
                Towplane = towplane,
                Glider = glider,
                }
            ]);

            string expectedCsv;
            using (var sr = new StreamReader("Operation\\GetExportToCsvOperation.csv"))
            {
                expectedCsv = sr.ReadToEnd();
            }

            var expectedOutput = Encoding.UTF8.GetBytes(expectedCsv);
            
            // Act
            var result = getExportToCsvOperation.Execute();

            // Assert
            Assert.Equal(expectedOutput, result);
        }
    }
}

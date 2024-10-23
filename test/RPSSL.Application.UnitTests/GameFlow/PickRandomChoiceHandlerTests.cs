using Moq;
using RPSSL.Domain.GameFlow;
using RPSSL.Domain.Exceptions;
using RPSSL.Domain.Abstraction;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Clients;
using RPSSL.Application.GameFlow.PickRandomChoice;

namespace RPSSL.Application.UnitTests.GameFlow;

public class PickRandomChoiceHandlerTests
{
    private readonly PickRandomChoiceHandler _handler;
    private readonly Mock<IRandomNumberApiClient> _randomNumberApiClientMock;

    public PickRandomChoiceHandlerTests()
    {
        _randomNumberApiClientMock = new Mock<IRandomNumberApiClient>();
        _handler = new PickRandomChoiceHandler(_randomNumberApiClientMock.Object);
    }

    [Theory]
    [InlineData(Choice.Rock, "rock")]
    [InlineData(Choice.Paper, "paper")]
    [InlineData(Choice.Scissors, "scissors")]
    public async Task Handle_ShouldReturnRandomChoice_WhenClientReturnsChoice(Choice inputChoice, string expectedChoiceName)
    {
        // Arrange
        var randomChoice = new Result<ChoiceResponse>(new ChoiceResponse((int)inputChoice, expectedChoiceName));

        _randomNumberApiClientMock
            .Setup(client => client.GetRandomChoiceAsync())
            .ReturnsAsync(randomChoice);

        var query = new PickRandomChoiceQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal((int)inputChoice, result.Value.Id);
        Assert.Equal(expectedChoiceName, result.Value.Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnException_WhenClientReturnsExceptionResult()
    {
        // Arrange
        var exceptionMessage = "Server unavailable.";
        var exceptionResult = new Result<ChoiceResponse>(new RandomNumberApiException(exceptionMessage));

        _randomNumberApiClientMock
            .Setup(client => client.GetRandomChoiceAsync())
            .ReturnsAsync(exceptionResult); // Mock client returning the exception wrapped in a Result

        var query = new PickRandomChoiceQuery();
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Exception);
        Assert.IsType<RandomNumberApiException>(result.Exception);
        Assert.Equal(exceptionMessage, result.Exception.Message);
    }
}



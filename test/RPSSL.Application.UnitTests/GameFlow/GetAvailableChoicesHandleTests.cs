using RPSSL.Application.GameFlow.GetAvailableChoices;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Domain.Abstraction;
using RPSSL.Domain.GameFlow;

namespace RPSSL.Application.UnitTests.GameFlow;

public class GetAvailableChoicesHandlerTests
{
    private readonly GetAvailableChoicesHandler _handler;

    public GetAvailableChoicesHandlerTests()
    {
        _handler = new GetAvailableChoicesHandler();
    }

    [Fact]
    public async Task Handle_ShouldReturnAvailableChoices()
    {
        // Arrange
        var query = new GetAvailableChoicesQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(Enum.GetValues(typeof(Choice)).Length, result.Value.Count);

        foreach (var choice in Enum.GetValues(typeof(Choice)).Cast<Choice>())
        {
            var expectedChoiceResponse = new ChoiceResponse((int)choice, choice.ToString().ToLower());
            Assert.Contains(result.Value, c => c.Id == expectedChoiceResponse.Id && c.Name == expectedChoiceResponse.Name);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnTaskResultSynchronously()
    {
        // Arrange
        var query = new GetAvailableChoicesQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.IsType<Result<IReadOnlyList<ChoiceResponse>>>(result);
    }
}

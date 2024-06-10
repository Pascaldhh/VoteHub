using Moq;
using Opinion8.Interfaces;
using Opinion8.Models;
using Opinion8.Services;

namespace Opinion8.Tests;

public class PollOptionServiceTests
{
    private const string UserId = "12345";

    public IPollOptionRepository SetupRepository()
    {
        Mock<IPollOptionRepository> mockPollOptionRepository = new();
        mockPollOptionRepository.Setup(x => x.Save(It.IsAny<PollOption>()));

        return mockPollOptionRepository.Object;
    }

    [
        Test,
        TestCase("option1, option2", ExpectedResult = 2),
        TestCase("option1, option2, option1, option2", ExpectedResult = 4),
        TestCase(
            "option1, option2, option1, option2, option1, option2, option1, option2",
            ExpectedResult = 8
        ),
        TestCase(
            "option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2",
            ExpectedResult = 16
        ),
        TestCase(
            "option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2",
            ExpectedResult = 32
        ),
        TestCase(
            "option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2, option1, option2",
            ExpectedResult = 64
        )
    ]
    public int SaveFromString_stringCommaSeperatedPollOptions_ReturnsListPollOptions(
        string optionsStr
    )
    {
        IPollOptionService pollOptionService = new PollOptionService(SetupRepository());
        IEnumerable<PollOption> listOfOptions = pollOptionService.SaveFromString(
            new Poll(),
            optionsStr
        );

        return listOfOptions.Count();
    }
}

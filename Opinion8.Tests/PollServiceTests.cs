using System.Security.Claims;
using Moq;
using Opinion8.Interfaces;
using Opinion8.Models;
using Opinion8.Repositories;
using Opinion8.Services;

namespace Opinion8.Tests;

public class PollServiceTests
{
    private const string UserId = "12345";

    public IPollRepository SetupRepository()
    {
        Mock<IPollRepository> mockPollRepository = new() { CallBase = true };
        mockPollRepository
            .Setup(x => x.GetAll())
            .Returns(
                [
                    new Poll
                    {
                        Question = "question 1",
                        HasVoted = false,
                        Options =
                        [
                            new PollOption
                            {
                                Name = "Option 1",
                                Votes = [new PollVote { UserId = UserId }]
                            }
                        ]
                    }
                ]
            );
        return mockPollRepository.Object;
    }

    [Test]
    public void GetAll_User_ReturnsPollWithCorrectHasVoted()
    {
        List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, UserId)];

        ClaimsIdentity identity = new(claims);
        ClaimsPrincipal claimsPrincipal = new(identity);

        PollService service = new(SetupRepository());

        IEnumerable<Poll> list = service.GetAll(claimsPrincipal);

        Assert.That(list.First().HasVoted, Is.True);
    }
}

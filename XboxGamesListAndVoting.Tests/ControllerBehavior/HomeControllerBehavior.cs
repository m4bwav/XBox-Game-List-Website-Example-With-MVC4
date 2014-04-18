using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using XboxGamesListAndVoting.Controllers;
using XboxGamesListAndVoting.NerderyXBoxGamesWebService;
using XboxGamesListAndVoting.Services;

namespace XboxGamesListAndVoting.Tests.ControllerBehavior
{
    [TestClass]
    public class HomeControllerBehavior
    {
        private const string SampleTitle = "a title";
        private IXBoxGameProxyService _gameService;
        private IVoteCheckService _voteService;

        private HomeController _homeController;

        [TestInitialize]
        public void Setup()
        {
            _gameService = MockRepository.GenerateStub<IXBoxGameProxyService>();
            _voteService = MockRepository.GenerateStub<IVoteCheckService>();
            _homeController = new HomeController(_gameService, _voteService);
        }

        [TestMethod]
        public void ShouldCallOwnGameLogic()
        {
            _homeController.OwnGame(SampleTitle);

            _gameService.AssertWasCalled(x => x.SetGameTitleToOwned(SampleTitle));
        }

        [TestMethod]
        public void ShouldCallGameServiceForVoting()
        {
            _voteService.Stub(x => x.CanUserVote()).Return(true);

            _gameService.Stub(x => x.GetXBoxGameByTitle(SampleTitle)).Return(new XboxGame());

            _homeController.VoteForGame(SampleTitle);

            _gameService.AssertWasCalled(x => x.IssueVoteForTitle(SampleTitle));
        }

        [TestMethod]
        public void ShouldRetrieveGamesAndCheckVotingStatusOnIndexAction()
        {
            _gameService.Stub(x => x.GetOwnedGames()).Return(new XboxGame[0]);
            _gameService.Stub(x => x.GetUnownedGamesByVotes()).Return(new XboxGame[0].OrderBy(x => x.Title));

            _homeController.Index();

            _gameService.AssertWasCalled(x => x.GetOwnedGames());
            _gameService.AssertWasCalled(x => x.GetUnownedGamesByVotes());
            _voteService.AssertWasCalled(x => x.CanUserVote());
        }
    }
}

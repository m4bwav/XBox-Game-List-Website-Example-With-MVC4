using System.Linq;
using System.Web.Mvc;
using XboxGamesListAndVoting.Models;
using XboxGamesListAndVoting.Services;

namespace XboxGamesListAndVoting.Controllers
{
    /// <summary>
    /// Default Home controller for the web site.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IXBoxGameProxyService _gameService;
        private readonly IVoteCheckService _voteCheckService;

        public HomeController(IXBoxGameProxyService gameService, IVoteCheckService voteCheckService)
        {
            _gameService = gameService;
            _voteCheckService = voteCheckService;
        }

        //
        // GET: /Home/
        /// <summary>
        /// Retrieves a list of games the Nerdery is voting on for purchase and those it has already purchased.
        /// </summary>
        /// <returns>A view and model</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var listModel = PrepareListOutputModel();

            return View(listModel);
        }


        /// <summary>
        /// Retrieves game lists and voting status and prepares an output model
        /// </summary>
        /// <returns>An output model ready for the game list page</returns>
        private NerderyGameCollectionDisplay PrepareListOutputModel()
        {
            var votingGames = _gameService
                .GetUnownedGamesByVotes()
                .Select(x => new XboxGameDisplay(x));

            var ownedGames = _gameService
                .GetOwnedGames()
                .Select(x => new XboxGameDisplay(x));

            var canVote = _voteCheckService.CanUserVote();

            return new NerderyGameCollectionDisplay(votingGames, ownedGames, canVote);
        }

        /// <summary>
        /// Retrieves the details about a particular game including ownership and votes.
        /// </summary>
        /// <param name="title">The title of the game to view.</param>
        [HttpGet]
        public ActionResult ViewGame(string title)
        {
            var selectedGame = _gameService.GetXBoxGameByTitle(title);

            if (selectedGame == null)
                return RedirectToAction("Index");

            var model = new XboxGameDisplay(selectedGame);

            return View(model);
        }


        /// <summary>
        /// Selects a game to denote as purchased.
        /// </summary>
        /// <param name="title">The title of the game that has been purchased</param>
        [HttpGet]
        public ActionResult OwnGame(string title)
        {
            _gameService.SetGameTitleToOwned(title);

            TempData.Add("SuccessMessage", title + " was added to the Nerdery Collection successfully.");

            return RedirectToAction("ViewGame", new {title});
        }
        
        /// <summary>
        /// Adds a new game title to the Nerdery's voting list.
        /// </summary>
        /// <param name="newGame">A new game title to add to the Nerdery's voting list</param>
        [HttpPost]
        public ActionResult AddNewGame(NewXboxGameDisplay newGame)
        {
            var outputModel = PrepareListOutputModel();

            var game = _gameService.GetXBoxGameByTitle(newGame.NewTitle);

            if(game != null)
                ModelState.AddModelError("NewGame.NewTitle", "This title has already been added to the Nerdery game list.");

            if (!ModelState.IsValid)
                return View("Index", outputModel);

            _gameService.AddNewTitle(newGame.NewTitle);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Issue a single 
        /// </summary>
        /// <param name="title">Title user wishes to vote on.</param>
        [HttpGet]
        public ActionResult VoteForGame(string title)
        {
            var canVote = _voteCheckService.CanUserVote();

            if (!canVote)
                return RedirectToAction("Index");

            var selectedGame = _gameService.GetXBoxGameByTitle(title);

            if (selectedGame == null)
                return RedirectToAction("Index");

            _gameService.IssueVoteForTitle(title);

            return RedirectToAction("Index");
        }
    }
}
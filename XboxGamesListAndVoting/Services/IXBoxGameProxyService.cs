using System;
using System.Collections.Generic;
using System.Linq;
using XboxGamesListAndVoting.NerderyXBoxGamesWebService;

namespace XboxGamesListAndVoting.Services
{
    /// <summary>
    /// This interface is used to handle communication with a data source for the purposes of tracking the Nerdery's XBox game list
    /// </summary>
    public interface IXBoxGameProxyService
    {
        /// <summary>
        /// Retrieves a list of games that can be voted on.  Games must have already been added to the Nerdery voting list first.
        /// </summary>
        /// <returns>A list of unowned games for voting</returns>
        IOrderedEnumerable<XboxGame> GetUnownedGamesByVotes();

        /// <summary>
        /// Retrieves the list of games that the Nerdery owns.
        /// </summary>
        /// <returns>A list of games that the Nerdery owns</returns>
        IEnumerable<XboxGame> GetOwnedGames();

        /// <summary>
        /// Retrieve an existing Xbox game by title.  
        /// </summary>
        /// <param name="title">Title of an existing game</param>
        /// <returns>The game or null if the game could not be found</returns>
        XboxGame GetXBoxGameByTitle(string title);

        /// <summary>
        ///     Marks a game in the Nerdery list as owned now, and takes it out of voting.
        /// </summary>
        /// <param name="title">The title of a game that exists in the voting list</param>
        void SetGameTitleToOwned(string title);

        /// <summary>
        ///     Add a new title to the Nerdery Library
        /// </summary>
        /// <param name="title">Title of new game to add to game list for voting.</param>
        void AddNewTitle(string title);

        /// <summary>
        ///     Causes a vote to be issued for a particular title.
        /// </summary>
        /// <param name="title">The title of the game the vote is for</param>
        void IssueVoteForTitle(string title);
    }

    /// <summary>
    /// This implementation of the IXBoxGameProxyService connects to the Nerdery's existing web services to handle Xbox game tracking.
    /// </summary>
    public class XBoxGameWebServiceProxy : IXBoxGameProxyService
    {
        private const string EndpointConfigurationName = "XboxVotingServiceSoap";
        private readonly string _apiKey;
        private readonly IVoteCheckService _voteCheckService;
        private readonly XboxVotingServiceSoapClient _webServiceClient;

        /// <summary>
        /// Initializes the XBoxGameWebServiceProxy
        /// </summary>
        /// <param name="configReader">A reader to retrieve configuration settings</param>
        /// <param name="voteCheckService">A service to track whether a vote has been issues and when.</param>
        public XBoxGameWebServiceProxy(IConfigurationReader configReader, IVoteCheckService voteCheckService)
        {
            _voteCheckService = voteCheckService;
            _apiKey = configReader.ApiKey;
            _webServiceClient = new XboxVotingServiceSoapClient(EndpointConfigurationName);
        }

        /// <summary>
        /// Retrieves a list of games that can be voted on.  Games must have already been added to the Nerdery voting list first.
        /// </summary>
        /// <returns>A list of unowned games for voting</returns>
        public IOrderedEnumerable<XboxGame> GetUnownedGamesByVotes()
        {
            return _webServiceClient
                .GetGames(_apiKey)
                .Where(x => x.Status != XboxGame.GameOwnedStatus)
                .OrderByDescending(x => x.Votes);
        }

        /// <summary>
        /// Retrieves the list of games that the Nerdery owns.
        /// </summary>
        /// <returns>A list of games that the Nerdery owns</returns>
        public IEnumerable<XboxGame> GetOwnedGames()
        {
            return _webServiceClient.GetGames(_apiKey)
                .Where(x => x.Status == XboxGame.GameOwnedStatus)
                .OrderBy(x => x.Title);
        }

        /// <summary>
        /// Retrieve an existing Xbox game by title.  
        /// </summary>
        /// <param name="title">Title of an existing game</param>
        /// <returns>The game or null if the game could not be found</returns>
        public XboxGame GetXBoxGameByTitle(string title)
        {
            return _webServiceClient.GetGames(_apiKey)
                .SingleOrDefault(x => x.Title == title);
        }

        /// <summary>
        ///     Marks a game in the Nerdery list as owned now, and takes it out of voting.
        /// </summary>
        /// <param name="title">The title of a game that exists in the voting list</param>
        public void SetGameTitleToOwned(string title)
        {
            var game = GetXBoxGameByTitle(title);

            _webServiceClient.SetGotIt(game.Id, _apiKey);
        }

        /// <summary>
        ///     Add a new title to the Nerdery Library
        /// </summary>
        /// <param name="title">Title of new game to add to game list for voting.</param>
        public void AddNewTitle(string title)
        {
            if (!_voteCheckService.CanUserVote())
                throw new InvalidOperationException("This user has already taken a voting action today");

            var trimmedTitle = title.Trim();

            var game = GetXBoxGameByTitle(trimmedTitle);

            if (game != null)
                throw new InvalidOperationException("This game is already in the list of games");

            _webServiceClient.AddGame(trimmedTitle, _apiKey);

            _voteCheckService.UpdateLastVoteTime();
        }

        /// <summary>
        ///     Causes a vote to be issued for a particular title.
        /// </summary>
        /// <param name="title">The title of the game the vote is for</param>
        public void IssueVoteForTitle(string title)
        {
            var trimmedTitle = title.Trim();

            var game = GetXBoxGameByTitle(trimmedTitle);

            if (game == null)
                throw new ArgumentOutOfRangeException("title",
                    "A game title selected for voting did not exist in the list.");

            if (!_voteCheckService.CanUserVote())
                throw new InvalidOperationException("A vote should not have been issued by this user");

            _webServiceClient.AddVote(game.Id, _apiKey);

            _voteCheckService.UpdateLastVoteTime();
        }
    }
}
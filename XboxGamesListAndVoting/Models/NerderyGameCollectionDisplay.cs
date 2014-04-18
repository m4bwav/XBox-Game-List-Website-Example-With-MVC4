using System.Collections.Generic;

namespace XboxGamesListAndVoting.Models
{
    /// <summary>
    /// A display model for showing the whole Nerdery game collection and the games that are being voted on.
    /// </summary>
    public class NerderyGameCollectionDisplay
    {
        /// <summary>
        /// Initializes a new instance of the display model.
        /// </summary>
        /// <param name="unownedGames">Games that are being voted on.</param>
        /// <param name="ownedGames">Games the Nerdery owns.</param>
        /// <param name="canVote">Is the user allowed to issue a vote.</param>
        public NerderyGameCollectionDisplay(IEnumerable<XboxGameDisplay> unownedGames,
            IEnumerable<XboxGameDisplay> ownedGames,
            bool canVote)
        {
            OwnedGames = new XboxGameListDisplay(ownedGames);
            UnownedGames = new XboxGameListDisplay(unownedGames, canVote);
            CanAddNewGame = canVote;
        }
        /// <summary>
        /// The list of games to vote on
        /// </summary>
        public XboxGameListDisplay UnownedGames { get; private set; }

        /// <summary>
        /// A list of games owned by the Nerdery
        /// </summary>
        public XboxGameListDisplay OwnedGames { get; private set; }

        /// <summary>
        /// Title of a new game to be added to the list
        /// </summary>
        public NewXboxGameDisplay NewGame { get; set; }

        /// <summary>
        /// Does the user have the ability to add new games
        /// </summary>
        public bool CanAddNewGame { get; set; }
    }
}
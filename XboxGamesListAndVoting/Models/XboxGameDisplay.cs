using XboxGamesListAndVoting.NerderyXBoxGamesWebService;

namespace XboxGamesListAndVoting.Models
{
    /// <summary>
    /// Display model for showing data about an Xbox game
    /// </summary>
    public class XboxGameDisplay
    {
        /// <summary>
        /// Intialize a new Game display model
        /// </summary>
        /// <param name="game">The game that will be displayed</param>
        public XboxGameDisplay(XboxGame game)
        {
            Votes = game.Votes;
            Title = game.Title;
            IsOwned = game.IsOwned;
        }


        /// <summary>
        /// Is the game owned
        /// </summary>
        public bool IsOwned { get; private set; }

        /// <summary>
        /// Number of votes the game has
        /// </summary>
        public int Votes { get; private set; }

        /// <summary>
        /// The title of the game.
        /// </summary>
        public string Title { get; private set; }
    }
}
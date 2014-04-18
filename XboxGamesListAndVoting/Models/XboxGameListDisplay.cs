using System.Collections.Generic;

namespace XboxGamesListAndVoting.Models
{
    public class XboxGameListDisplay
    {
        /// <summary>
        /// Creates a non-voting list of games
        /// </summary>
        /// <param name="games">A list of games</param>
        public XboxGameListDisplay(IEnumerable<XboxGameDisplay> games) : this(games, false)
        {
        }

        /// <summary>
        /// Creates an xbox model list from the xbox games passed in, and sets the canvote flag.
        /// </summary>
        /// <param name="games">A list of games</param>
        /// <param name="canVote">Whether the current user can vote on these games</param>
        public XboxGameListDisplay(IEnumerable<XboxGameDisplay> games, bool canVote)
        {
            Games = games;
            CanVote = canVote;
        }

        /// <summary>
        /// A list of game display models.
        /// </summary>
        public IEnumerable<XboxGameDisplay> Games { get; private set; }

        /// <summary>
        /// Whether the current user can issue a vote on this list of games.
        /// </summary>
        public bool CanVote { get; private set; }
    }
}
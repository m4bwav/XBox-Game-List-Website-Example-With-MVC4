using System;
using System.Web;

namespace XboxGamesListAndVoting.Services
{
    /// <summary>
    /// An interface to vote tracking
    /// </summary>
    public interface IVoteCheckService
    {
        /// <summary>
        /// Determines if a user is still eligible to vote
        /// </summary>
        /// <returns>Whether the current user can vote.</returns>
        bool CanUserVote();

        /// <summary>
        /// Records a new last voted time for the current user.
        /// </summary>
        void UpdateLastVoteTime();
    }


    /// <summary>
    /// This vote tracking implementation uses cookies to denote whether a user has voted.
    /// </summary>
    public class VoteCheckCookieService : IVoteCheckService
    {
        private const string LastNerderyXboxVoteKey = "LastNerderyXboxVoteKey";
        private const string LastVotedOnKey = "LastVotedOn";

        /// <summary>
        /// Checks if the voting is on an eligible date and whether more than 24 hours has passed since the last vote by reading a cookie.
        /// </summary>
        /// <returns>True if the user can still vote, false otherwise</returns>
        public bool CanUserVote()
        {
            var now = DateTime.Now;

            var dayOfTheWeek = now.DayOfWeek;

            if (dayOfTheWeek == DayOfWeek.Saturday || dayOfTheWeek == DayOfWeek.Sunday)
                return false;

            var timeCookie = HttpContext.Current.Request.Cookies[LastNerderyXboxVoteKey];

            if (timeCookie == null)
                return true;

            var lastVotedOn = DateTime.Parse(timeCookie[LastVotedOnKey]);

            return now.Subtract(lastVotedOn).TotalHours >= 24;
        }

        /// <summary>
        /// Update the last time of a vote in a cookie for that purpose.
        /// </summary>
        public void UpdateLastVoteTime()
        {
            var httpCookie = new HttpCookie(LastNerderyXboxVoteKey);

            httpCookie[LastVotedOnKey] = DateTime.Now.ToString();

            HttpContext.Current.Response.SetCookie(httpCookie);
        }
    }
}
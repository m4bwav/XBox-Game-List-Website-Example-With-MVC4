namespace XboxGamesListAndVoting.NerderyXBoxGamesWebService
{
    /// <summary>
    /// A partial class for the Nerdery web service's primary object.  This is mostly for adding convenience methods and properties.
    /// </summary>
    public partial class XboxGame
    {
        /// <summary>
        /// Nerdery status string that denotes ownership.
        /// </summary>
        public const string GameOwnedStatus = "gotit";

        /// <summary>
        /// Whether a particular XBox game is owned.
        /// </summary>
        public bool IsOwned
        {
            get { return Status == GameOwnedStatus; }
        }
    }
}
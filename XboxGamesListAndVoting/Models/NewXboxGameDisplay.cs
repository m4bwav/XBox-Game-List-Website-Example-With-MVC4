using System.ComponentModel.DataAnnotations;

namespace XboxGamesListAndVoting.Models
{
    /// <summary>
    /// An model for handling display and input of new games
    /// </summary>
    public class NewXboxGameDisplay
    {
        /// <summary>
        /// A new title to be added to the voting list.
        /// </summary>
        [Display(Name = "Game Title")]
        [Required(ErrorMessage = "Game title is required")]
        public string NewTitle { get; set; }
    }
}
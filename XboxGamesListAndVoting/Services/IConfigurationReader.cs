using System.Configuration;

namespace XboxGamesListAndVoting.Services
{
    /// <summary>
    /// An interface for reading from the application's configuration
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// The Nerdery API key for web services calls
        /// </summary>
        string ApiKey { get; }
    }

    /// <summary>
    /// Concrete implementation of a config file reader.  
    /// This reader should normally reads from the Web.config file, but it can also read an App.config file of a non-web app
    /// </summary>
    public class ConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// The Nerdery API key for web services calls
        /// </summary>
        public string ApiKey
        {
            get { return ConfigurationManager.AppSettings["NerderyApiKey"]; }
        }
    }
}
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuleapDocGenerator
{
    /// <summary>
    /// General settings of the application.
    /// </summary>
    public class Settings
    {
        private static Settings? msInstance;

        /// <summary>
        /// Logger of the class.
        /// </summary>
        private static readonly Logger msLogger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the Tuleap Uri.
        /// </summary>
        public string? TuleapUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Tuleap API Key.
        /// </summary>
        public string? TuleapKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the root tracker ID.
        /// </summary>
        public List<int> TrackerIds
        {
            get;
            set;
        }

        /// <summary>
        /// Directory where the data are stored.
        /// </summary>
        public string? TemplateDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the unique instance of the settings.
        /// </summary>
        public static Settings GetInstance()
        {
            if (msInstance == null)
            {
                // If the setting is null, create default one.
                if (File.Exists(@".\settings.json"))
                {
                    try
                    {
                        string lJsonValue = File.ReadAllText(@".\settings.json");
                        msInstance = JsonConvert.DeserializeObject<Settings>(lJsonValue);
                    }
                    catch (Exception lEx)
                    {
                        msLogger.Log(LogLevel.Error, "error:@Cannot load settings.json" + lEx);
                    }
                }
                else
                {
                    msLogger.Log(LogLevel.Error, "Cannot find settings.json");
                }
                if (msInstance == null)
                {
                    msInstance = new Settings
                    {
                        TuleapUri = "https://tuleap.net/api/",
                        TuleapKey = "tlp-k1-74.0938e677298d61a90d7a50246dfbce060eaa752b7298de23f5b233569aca766a",
                        TrackerIds = new List<int>() { 812, 813, 814 },
                        TemplateDirectory = @".\templates\",
                    };
                    Formatting lIndented = Formatting.Indented;
                    string? lSerialized = JsonConvert.SerializeObject(msInstance, lIndented);

                    File.WriteAllText(@".\settings.json", lSerialized);
                }
            }
            return msInstance;
        }
    }
}

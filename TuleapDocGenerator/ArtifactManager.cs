using System.Collections.ObjectModel;
using System.ComponentModel;
using NLog;
using XTuleap;

namespace TuleapDocGenerator
{
    public sealed class ArtifactManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Logger of the class.
        /// </summary>
        private static readonly Logger msLogger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Event raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        // The Singleton's instance is stored in a static field. There there are
        // multiple ways to initialize this field, all of them have various pros
        // and cons. In this example we'll show the simplest of these ways,
        // which, however, doesn't work really well in multi-threaded program.
        private static ArtifactManager? msInstance;

        /// <summary>
        /// This field stores all the loaded trackers.
        /// </summary>
        public ObservableCollection<Tracker<Artifact>> mTrackers = new ObservableCollection<Tracker<Artifact>>();

        /// <summary>
        /// This property stores the Tuleap connection.
        /// </summary>
        public Connection? Connection
        {
            get;
            set;
        }

        /// <summary>
        /// Checks if the manager is initialized.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.Connection != null;
            }
        }

        /// <summary>
        /// Gets the last error returned by tuleap.
        /// </summary>
        public string? LastError
        {
            get;
            private set;
        }

        // This is the static method that controls the access to the singleton
        // instance. On the first run, it creates a singleton object and places
        // it into the static field. On subsequent runs, it returns the client
        // existing object stored in the static field.
        public static ArtifactManager GetInstance()
        {
            if (msInstance == null)
            {
                msInstance = new ArtifactManager();
            }
            return msInstance;
        }

        // The Singleton's constructor should always be private to prevent
        // direct construction calls with the `new` operator.
        private ArtifactManager()
        {
        }

        /// <summary>
        /// Initialize the application.
        /// </summary>
        /// <param name="pConnectToTuleap">A flag to connect to tuleap</param>
        public RootTracker Initialize(bool pConnectToTuleap)
        {
            RootTracker lResult = new RootTracker();

            DateTime lStart = DateTime.Now;
            try
            {

                msLogger.Log(LogLevel.Info, "Start tuleap initialization");
                this.Connection = new Connection(Settings.GetInstance().TuleapUri, Settings.GetInstance().TuleapKey);
                if (this.Connection != null)
                {
                    msLogger.Log(LogLevel.Info, "Connection established.");
                    foreach (var lTrackerId in Settings.GetInstance().TrackerIds)
                    {
                        TrackerStructure lStructure = this.Connection.AddTrackerStructure(lTrackerId);
                        Tracker<Artifact> lTracker = new Tracker<Artifact>(lStructure);
                        lTracker.PreviewRequest(this.Connection);
                        lTracker.Request(this.Connection);
                        Console.WriteLine("Tracker structure loaded : " + lTrackerId);
                        this.mTrackers.Add(lTracker);
                    }
                }

                msLogger.Log(LogLevel.Info, "Tuleap initialized in " + (DateTime.Now - lStart).TotalSeconds + " seconds.");
            }
            catch
            {
                this.Connection = null;
                this.LastError = "URL or SS key invalid. Please check the them in the settings.";
                msLogger.Log(LogLevel.Error, "URL or SS key invalid. Please check the them in the settings.");
            }

            Tracker<Artifact> lRootTracker = this.mTrackers.FirstOrDefault(pTracker => pTracker.Structure.Id == 812);
            if (lRootTracker != null)
            {
                lResult.TrackerName = lRootTracker.Name;
                foreach (var lArtifact in lRootTracker.Artifacts)
                {
                    lResult.Records.Add(new TrackerRecord(lArtifact));
                }
            }

            return lResult;
        }

        /// <summary>
        /// Get an artifact by its ID.
        /// </summary>
        /// <param name="pArtifactId">The artifact id.</param>
        /// <returns>The artifact if exists, null otherwise.</returns>
        public Artifact? GetArtifact(int pArtifactId)
        {
            foreach (var lTracker in this.mTrackers)
            {
                foreach (var lArtifact in lTracker.Artifacts)
                {
                    if (lArtifact.Id == pArtifactId)
                    {
                        return lArtifact;
                    }
                }
            }
            return null;
        }
    }
}
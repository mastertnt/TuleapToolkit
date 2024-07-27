using DocumentFormat.OpenXml.Wordprocessing;
using SharpDocx;
using System.Text;
using System.Windows;
using XTuleap;

namespace TuleapDocGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This property stores the Tuleap connection.
        /// </summary>
        public Connection? Connection
        {
            get;
            set;
        }

        private RootTracker mRootTracker;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.Connection = new Connection(Settings.GetInstance().TuleapUri, Settings.GetInstance().TuleapKey);
            if (this.Connection != null)
            {
                // Now, build the RootTracker model.
                
                foreach (var lTracker in ArtifactManager.GetInstance().mTrackers)
                {
                    foreach (var lArtifact in lTracker.Artifacts)
                    {
                        this.mRootTracker.Records.Add(new TrackerRecord(lArtifact));
                    }
                }
            }
        }

        /// <summary>
        /// Method called when the generate button is clicked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGenerateClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            try
            {
                RootTracker lRootTracker = ArtifactManager.GetInstance().Initialize(true);
                var lDocument = DocumentFactory.Create(@"d:\temp\documents\model.view.docx", lRootTracker);
                lDocument.Generate(@"d:\temp\documents\output.docx");
            }
            catch (SharpDocxCompilationException lException)
            {
                StringBuilder lStringBuilder = new StringBuilder(lException.Errors);
                lStringBuilder.AppendLine(lException.SourceCode);
                Console.WriteLine(lStringBuilder.ToString());
            }
        }
    }
}
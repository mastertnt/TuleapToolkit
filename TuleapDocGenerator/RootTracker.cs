using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuleapDocGenerator
{
    /// <summary>
    /// This class stores all records of the root tracker.
    /// </summary>
    public class RootTracker
    {
        public List<TrackerRecord> Records { get; set; }

        public string TrackerName { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RootTracker()
        {
            this.Records = new List<TrackerRecord>();
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>All records associated with this tracker.</returns>
        public List<TrackerRecord> GetRecords()
        {
            return this.Records;
        }
    }
}

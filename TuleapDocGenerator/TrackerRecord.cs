using XTuleap;

namespace TuleapDocGenerator
{
    /// <summary>
    /// This class store a tracker record.
    /// </summary>
    public class TrackerRecord
    {
        private Artifact mArtifact;

        private List<TrackerRecord>? mChildren = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TrackerRecord(Artifact pArtifact)
        {
            this.mArtifact = pArtifact;
        }

        /// <summary>
        /// Gets all attached records as children.
        /// </summary>
        /// <returns></returns>
        public List<TrackerRecord> GetChildren()
        {
            if (this.mChildren == null)
            {
                this.mChildren = new List<TrackerRecord>();
                List<ArtifactLink>? lLinks = this.mArtifact.GetFieldValue<List<ArtifactLink>?>("artifact_links");
                if (lLinks != null)
                {
                    foreach (var lChild in lLinks)
                    {
                        Artifact? lArtifact = ArtifactManager.GetInstance().GetArtifact(lChild.Id);
                        if (lArtifact != null)
                        {
                            this.mChildren.Add(new TrackerRecord(lArtifact));
                        }
                    }
                }
            }

            return this.mChildren;
        }

        /// <summary>
        /// Gets the tracker name.
        /// </summary>
        /// <returns>The tracker name as string.</returns>
        public string GetTrackerName()
        {
            return this.mArtifact.TrackerName;
        }

        /// <summary>
        /// Get the value of a field as a string
        /// </summary>
        /// <param name="pFieldKey">The field key.</param>
        /// <returns>The string representation of the field value</returns>
        public string GetValueAsString(string pFieldKey)
        {
            return this.mArtifact.GetFieldValueAsString(pFieldKey);
        }
    }
}

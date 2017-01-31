using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack
{
    /// <summary>
    /// A specific task the user wants to track time to.
    /// </summary>
    public class TimeReport : List<TimeEntry>
    {
        /// <summary>
        /// Id to reference this report by.
        /// </summary>
        public uint Id
        {
            get;
            private set;
        }

        /// <summary>
        /// THe name of this report.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="id">Id of the new report.</param>
        /// <param name="name">Name of the new report.</param>
        public TimeReport(uint id, string name) : base()
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// Calculates the total time this report has tracked.
        /// </summary>
        public TimeSpan TotalTime
        {
            get
            {
                TimeSpan total = new TimeSpan(0);

                TimeEntry currentStartEntry = null;
                foreach(TimeEntry entry in this)
                {
                    if(currentStartEntry != null)
                    {
                        if(entry.EntryType == TimeEntryType.EndEntry)
                        {
                            total.Add(entry.Timestamp - currentStartEntry.Timestamp);
                            currentStartEntry = null;
                        }
                    }
                    else
                    {
                        if (entry.EntryType == TimeEntryType.StartEntry)
                            currentStartEntry = entry;
                    }
                }

                if (currentStartEntry != null)
                    total = DateTime.UtcNow - currentStartEntry.Timestamp;

                return total;
            }
        }

        /// <summary>
        /// Calculates how long the last actve time of this report was or currently is.
        /// </summary>
        public TimeSpan LatestActiveTime
        {
            get
            {
                TimeEntry lastStart = null;
                TimeEntry lastStop = null;

                foreach (TimeEntry entry in this)
                {
                    if(entry.EntryType == TimeEntryType.StartEntry)
                    {
                        lastStart = entry;
                        lastStop = null;
                    }
                    else if (entry.EntryType == TimeEntryType.EndEntry)
                    {
                        lastStop = entry;
                    }
                }

                if (lastStart == null)
                    return TimeSpan.Zero;
                else if (lastStop == null)
                    return DateTime.UtcNow - lastStart.Timestamp;
                else
                    return lastStop.Timestamp - lastStart.Timestamp;
            }
        }
    }
}

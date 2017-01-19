using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack
{
    /// <summary>
    /// An Entry in a time report.
    /// </summary>
    public class TimeEntry
    {
        /// <summary>
        /// The type of entry.
        /// </summary>
        public TimeEntryType EntryType
        {
            get;
            set;
        }

        /// <summary>
        /// The time of the entry.
        /// </summary>
        public DateTime Timestamp
        {
            get;
            set;
        }
    }
}

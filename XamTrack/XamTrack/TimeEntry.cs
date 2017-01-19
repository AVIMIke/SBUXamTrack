using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack
{
    public class TimeEntry
    {
        public TimeEntryType EntryType
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }
    }
}

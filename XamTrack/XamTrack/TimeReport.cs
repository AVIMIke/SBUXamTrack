using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack
{
    public class TimeReport : List<TimeEntry>
    {
        public uint Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public TimeReport(uint id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}

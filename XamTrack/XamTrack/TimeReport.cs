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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack
{

    /// <summary>
    /// Handles all the reports. Users should use this instead of making new reports.
    /// </summary>
    public class ReportManager
    {
        private Dictionary<uint, TimeReport> _reports;
        private TimeReport _activeReport;

        private static uint nextReportId = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportManager()
        {
            _reports = new Dictionary<uint, TimeReport>();
        }

        /// <summary>
        /// Adds a new time report.
        /// </summary>
        /// <param name="reportName">The name of the new time report</param>
        public void AddReport(string reportName)
        {
            TimeReport r = new TimeReport(GenerateReportId(), reportName);
            _reports.Add(r.Id, r);
        }

        /// <summary>
        /// Removes a report.
        /// </summary>
        /// <param name="reportId">The id of the report to remove.</param>
        public void RemoveReport(uint reportId)
        {
            _reports.Remove(reportId);
        }

        /// <summary>
        /// Gets a summary of all the reports in the system.
        /// </summary>
        /// <returns>A list of all the reports in the system.</returns>
        public List<TimeReport> GetReportSummary()
        {
            return new List<TimeReport>(_reports.Values);
        }

        /// <summary>
        /// Starts tracking time in a report.
        /// </summary>
        /// <param name="reportId">The id of the report to start tracking.</param>
        public void StartTrackingReport(uint reportId)
        {
            if (!_reports.ContainsKey(reportId))
                return;

            if (_activeReport != null)
                StopTrackingReport();

            _activeReport = _reports[reportId];

            TimeEntry newEntry = new TimeEntry();
            newEntry.EntryType = TimeEntryType.StartEntry;
            newEntry.Timestamp = DateTime.UtcNow;
            _activeReport.Add(newEntry);
        }

        /// <summary>
        /// Stops tracking time in any active report.
        /// </summary>
        public void StopTrackingReport()
        {
            if (_activeReport == null)
                return;

            TimeEntry newEntry = new TimeEntry();
            newEntry.EntryType = TimeEntryType.EndEntry;
            newEntry.Timestamp = DateTime.UtcNow;
            _activeReport.Add(newEntry);

            _activeReport = null;
        }

        /// <summary>
        /// Generates a new unused report it.
        /// </summary>
        /// <returns>A new report id that is unused.</returns>
        private uint GenerateReportId()
        {
            uint rId = nextReportId;
            nextReportId++;
            return rId;
        }
    }
}

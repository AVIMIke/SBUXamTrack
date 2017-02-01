using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamTrack.SystemServices;

namespace XamTrack
{

    /// <summary>
    /// Handles all the reports. Users should use this instead of making new reports.
    /// </summary>
    public class ReportManager : INotifyPropertyChanged
    {
        private static ReportManager _instance = null;
        public static ReportManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ReportManager();

                return _instance;
            }
        }

        private Dictionary<uint, TimeReport> _reports;
        private TimeReport _activeReport;

        private static uint nextReportId = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportManager()
        {
            _reports = new Dictionary<uint, TimeReport>();

            this.AddReport("Test1");

            this.AddReport("Test2");

            this.AddReport("Test3");

            this.AddReport("Test4");

        }

        /// <summary>
        /// Adds a new time report.
        /// </summary>
        /// <param name="reportName">The name of the new time report</param>
        public void AddReport(string reportName)
        {
            TimeReport r = new TimeReport(GenerateReportId(), reportName);
            _reports.Add(r.Id, r);

            this.RaisePropertyChanged("ReportList");
        }

        /// <summary>
        /// Removes a report.
        /// </summary>
        /// <param name="reportId">The id of the report to remove.</param>
        public void RemoveReport(uint reportId)
        {
            _reports.Remove(reportId);

            this.RaisePropertyChanged("ReportList");
        }

        /// <summary>
        /// Gets a summary of all the reports in the system.
        /// </summary>
        /// <returns>A list of all the reports in the system.</returns>
        public List<TimeReport> GetAllReportSummary()
        {
            return _reports.Values.ToList();
        }

        /// <summary>
        /// Get the report the the system has Active.
        /// </summary>
        /// <returns>A reference to the active report.</returns>
        public TimeReport GetActiveReport()
        {
            return _activeReport;
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
            _activeReport.TimeEntrys.Add(newEntry);


            this.RaisePropertyChanged("ActiveReport");
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
            _activeReport.TimeEntrys.Add(newEntry);

            _activeReport = null;

            this.RaisePropertyChanged("ActiveReport");
        }

        /// <summary>
        /// Saves a flat file holding the data needed for this manager.
        /// The format is as follows
        /// 
        /// ActiveReportID (or -1 if no active report)
        /// XML Serialized list of TimeReports
        /// </summary>
        public void Save()
        {
            string data = "";

            if (_activeReport == null)
            {
                data += "-1\n";
            }
            else
            {
                data += _activeReport.Id + "\n";
            }

            List<TimeReport> saveReports = this.GetAllReportSummary();
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(saveReports.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                x.Serialize(textWriter, saveReports);
                data += textWriter.ToString();
            }

            ServiceContainer.FileService.WriteFile("ReportData.dat", data);
        }

        /// <summary>
        /// Loads the flat file holding the data needed for this manager.
        /// The expected format is as follows
        /// 
        /// ActiveReportID (or -1 if no active report)
        /// XML Serialized list of TimeReports
        /// </summary>
        public void Load()
        {
            this._reports.Clear();
            this._activeReport = null;

            string data = ServiceContainer.FileService.ReadFile("ReportData.dat");
            if (string.IsNullOrEmpty(data))
                return;

            int firstLineIndex = data.IndexOf("\n");
            string activeReport = data.Substring(0, firstLineIndex);
            string reportList = data.Substring(firstLineIndex+1);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<TimeReport>));

            List<TimeReport> loadedReports = null;
            try
            {
                using (TextReader reader = new StringReader(reportList))
                {
                    loadedReports = x.Deserialize(reader) as List<TimeReport>;
                }

                if (loadedReports != null)
                {
                    foreach (TimeReport r in loadedReports)
                    {
                        _reports.Add(r.Id, r);
                        nextReportId = r.Id + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
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

        /// <summary>
        /// Implementation of INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method to raise a property changed event in a safe way.
        /// </summary>
        /// <param name="prop"></param>
        private void RaisePropertyChanged(string prop)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));
        }
    }
}

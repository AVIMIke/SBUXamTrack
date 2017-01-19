using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack
{
    public class ReportManager
    {
        private Dictionary<uint, TimeReport> _reports;
        private TimeReport _activeReport;

        private static uint nextReportId = 0;

        public ReportManager()
        {
            _reports = new Dictionary<uint, TimeReport>();
        }

        public void AddReport(string reportName)
        {
            TimeReport r = new TimeReport(GenerateReportId(), reportName);
            _reports.Add(r.Id, r);
        }

        public void RemoveReport(uint reportId)
        {
            _reports.Remove(reportId);
        }

        public List<TimeReport> GetReportSummary()
        {
            return new List<TimeReport>(_reports.Values);
        }

        private uint GenerateReportId()
        {
            uint rId = nextReportId;
            nextReportId++;
            return rId;
        }
    }
}

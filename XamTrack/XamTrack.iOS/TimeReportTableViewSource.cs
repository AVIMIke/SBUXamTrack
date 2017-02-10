using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace XamTrack.iOS
{
	public class TimeReportTableViewSource : UITableViewSource
	{
		private List<TimeReport> _reports;
		private static string CellIdentifier = "TimeReportTableViewSource";

		public event EventHandler<TimeReport> ReportSelected;

		public TimeReportTableViewSource()
		{
			_reports = new List<TimeReport>();
		}

		public void SetReports(IEnumerable<TimeReport> r)
		{
			_reports.Clear();
			_reports.AddRange(r);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			TimeReport report = _reports[indexPath.Row];

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Value2, CellIdentifier);
			}

			cell.DetailTextLabel.Text = report.TotalTime.ToString("hh\\:mm\\:ss");
			cell.TextLabel.Text = report.Name;

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return _reports.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (ReportSelected != null)
				ReportSelected(this, _reports[indexPath.Row]);
		}
	}
}

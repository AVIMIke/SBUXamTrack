using System;
using System.Threading;
using CoreGraphics;
using Foundation;
using UIKit;

namespace XamTrack.iOS
{
	public partial class ViewController : UIViewController
	{
		TimeReportTableViewSource _reportAdapter;
		private Timer activeTaskUpdater;


		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			activeTaskUpdater = new Timer(HandleActiveTaskTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
			UpdateTaskTimer();

			ReportManager.Instance.PropertyChanged += ReportManager_PropertyChanged;

			_reportAdapter = new TimeReportTableViewSource();
			_reportAdapter.ReportSelected += ReportAdapter_ReportSelected;
			ReportList.Source = _reportAdapter;

			UILongPressGestureRecognizer longpressDetector = new UILongPressGestureRecognizer(HandleAction);
			ReportList.AddGestureRecognizer(longpressDetector);

			RefreshUI();
		}

		void HandleAction(UILongPressGestureRecognizer gesture)
		{
			CGPoint touch = gesture.LocationInView(ReportList);

			NSIndexPath indexPath = ReportList.IndexPathForRowAtPoint(touch);
			if (indexPath != null && gesture.State == UIGestureRecognizerState.Began)
			{
				LongPressOnReport(_reportAdapter.ReportAt(indexPath));
			}

		}

		void ReportAdapter_ReportSelected(object sender, TimeReport report)
		{
			if(ReportManager.Instance.GetActiveReport() == report)
			{
				ReportManager.Instance.StopTrackingReport();
			}
			else
			{
				ReportManager.Instance.StartTrackingReport(report.Id);
			}
		}

		void LongPressOnReport(TimeReport report)
		{
			ReportManager.Instance.RemoveReport(report.Id);
		}

		private void HandleActiveTaskTimerCallback(object state)
		{
			RefreshUI();
		}

		private void ReportManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			RefreshUI();

			if (e.PropertyName.Equals("ActiveReport") || e.PropertyName.Equals("Loaded"))
			{
				UpdateTaskTimer();
			}
		}

		void UpdateTaskTimer()
		{
			if (ReportManager.Instance.GetActiveReport() != null)
			{
				activeTaskUpdater.Change(100, 100);
			}
			else
			{
				activeTaskUpdater.Change(Timeout.Infinite, Timeout.Infinite);
			}
		}

		private void RefreshUI()
		{
			InvokeOnMainThread(() => 
			{
				_reportAdapter.SetReports(ReportManager.Instance.GetAllReportSummary());
				ReportList.ReloadData();

				TimeReport activeReport = ReportManager.Instance.GetActiveReport();

				if (activeReport == null)
				{
					ActiveTaskText.Text = "Select A Task To Track";
					ActiveTaskTotalTimeText.Text = "";
					ActiveTaskCurrentTimeText.Text = "";
				}
				else
				{
					ActiveTaskText.Text = activeReport.Name;
					ActiveTaskTotalTimeText.Text = activeReport.TotalTime.ToString("hh\\:mm\\:ss");
					ActiveTaskCurrentTimeText.Text = activeReport.LatestActiveTime.ToString("hh\\:mm\\:ss");
				}
			});
		}
	}

}
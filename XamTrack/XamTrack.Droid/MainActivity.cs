using System;
using System.Linq;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading;
using XamTrack.SystemServices;
using XamTrack.Droid.AndroidServices;

namespace XamTrack.Droid
{
    /// <summary>
    /// This activity will show the user a list of all thier time reports as well as the report that is currently being tracked.
    /// </summary>
	[Activity (Label = "XamTrack.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private Button addTaskButton;
        private TextView currentTaskName;
        private TextView currentTaskTime;
        private TextView totalTaskTime;
        private ListView taskList;
        private ArrayAdapter<string> taskAdapter;

		private Timer activeTaskUpdater;

        /// <summary>
        /// Override of Android Activity OnCreate
        /// </summary>
        /// <param name="bundle">The bundle that was passed when starting this activity.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            ServiceContainer.FileService = new AndroidFileService();
            ReportManager.Instance.Load();

			SetContentView (Resource.Layout.TrackingOverview);

            addTaskButton = FindViewById<Button>(Resource.TrackingOverview.AddTask);
            currentTaskName = FindViewById<TextView>(Resource.TrackingOverview.CurrentTaskName);
            currentTaskTime = FindViewById<TextView>(Resource.TrackingOverview.CurrentTaskTime);
            totalTaskTime = FindViewById<TextView>(Resource.TrackingOverview.TotalTaskTime);
            taskList = FindViewById<ListView>(Resource.TrackingOverview.TaskList);

            addTaskButton.Click += addTaskButton_Click;

            taskAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            taskList.Adapter = taskAdapter;
            taskList.ItemClick += taskList_ItemClick;
            taskList.ItemLongClick += taskList_ItemLongClick;

			activeTaskUpdater = new Timer(HandleActiveTaskTimerCallback, null, Timeout.Infinite, Timeout.Infinite);

            ReportManager.Instance.PropertyChanged += ReportManager_PropertyChanged;
		}

        /// <summary>
        /// Override of the Android Activity OnResume.
        /// This is part of the Activity lifecycle that is called any time the Actity is about to
        /// get shown on screen.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            this.RefreshUI();
        }

        /// <summary>
        /// Override of the Android Activity OnDestroy.
        /// This is part of the Activity lifecycle that is called any time the Actity is about to
        /// get removed from memory and shutdown.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            ReportManager.Instance.Save();

            addTaskButton.Click -= addTaskButton_Click;
            taskList.ItemClick -= taskList_ItemClick;
            taskList.ItemLongClick -= taskList_ItemLongClick;

			activeTaskUpdater.Dispose();
			activeTaskUpdater = null;

            ReportManager.Instance.PropertyChanged -= ReportManager_PropertyChanged;
        }

        /// <summary>
        /// Override of the Android Activity OnPause.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();

            ReportManager.Instance.Save();

            addTaskButton.Click -= addTaskButton_Click;
            taskList.ItemClick -= taskList_ItemClick;
            taskList.ItemLongClick -= taskList_ItemLongClick;

            //activeTaskUpdater.Dispose();
            //activeTaskUpdater = null;

            ReportManager.Instance.PropertyChanged -= ReportManager_PropertyChanged;
        }

        /// <summary>
        /// Handles the Add Task Button click
        /// </summary>
        /// <param name="sender">The button being clicked</param>
        /// <param name="e">Arguments about the click.</param>
        void addTaskButton_Click(object sender, EventArgs e)
        {
            ReportManager.Instance.AddReport("AddedReport");
        }

        /// <summary>
        /// Handles long presses on item in the task list
        /// </summary>
        /// <param name="sender">The list view sending this</param>
        /// <param name="e">Information about the longpress</param>
        void taskList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles "clicks" on item in the task list
        /// </summary>
        /// <param name="sender">The list view sending this</param>
        /// <param name="e">Information about the click</param>
        void taskList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TimeReport selected = ReportManager.Instance.GetAllReportSummary().ElementAt(e.Position);
            if(ReportManager.Instance.GetActiveReport() == selected)
            {
                ReportManager.Instance.StopTrackingReport();
            }
            else
            {
                ReportManager.Instance.StartTrackingReport(selected.Id);
            }
        }

        /// <summary>
        /// Property change listener for the ReportManager.
        /// </summary>
        /// <param name="sender">The report manager</param>
        /// <param name="e">A property changed event</param>
        void ReportManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RefreshUI();

            if(e.PropertyName.Equals("ActiveReport"))
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
        }

        /// <summary>
        /// Helper method that refreshes the whole UI based on the data in the model.
        /// </summary>
        private void RefreshUI()
        {
            this.RunOnUiThread(() =>
                {
                    // task list
                    taskAdapter.Clear();
                    foreach (TimeReport t in ReportManager.Instance.GetAllReportSummary())
                    {
                        taskAdapter.Add(t.Name);
                    }

                    TimeReport activeReport = ReportManager.Instance.GetActiveReport();
                    if (activeReport != null)
                    {
                        currentTaskName.Text = activeReport.Name;
                        currentTaskTime.Text = activeReport.LatestActiveTime.ToString("hh\\:mm\\:ss");
                        totalTaskTime.Text = activeReport.TotalTime.ToString("hh\\:mm\\:ss");
                    }
                    else
                    {
                        currentTaskName.Text = "Select a task to start tracking.";
                        currentTaskTime.Text = "";
                        totalTaskTime.Text = "";
                    }
                });
        }

		private void HandleActiveTaskTimerCallback(object state)
		{
			RefreshUI();
		}
	}
}



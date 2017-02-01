using System;
using System.Linq;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using System.Threading;
using XamTrack.SystemServices;
using XamTrack.Droid.AndroidServices;
using Android.Views;
using Android.Content;

namespace XamTrack.Droid
{
    /// <summary>
    /// This activity allows a user to add new reports to the report list.
    /// </summary>
    [Activity(Label = "XamTrack.Droid", MainLauncher = false, Icon = "@drawable/icon")]
    public class AddTaskActivity : Activity
    {
        private Button addTask;
        private Button cancel;
        private EditText nameText;

        /// <summary>
        /// Override of Android OnCreate
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddTask);

            nameText = FindViewById<EditText>(Resource.AddTask.NameInput);
            addTask = FindViewById<Button>(Resource.AddTask.Add);
            cancel = FindViewById<Button>(Resource.AddTask.Cancel);

            addTask.Click += addTask_Click;
            cancel.Click += cancel_Click;
        }

        /// <summary>
        /// Override of Android OnDestroy
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            addTask.Click -= addTask_Click;
            cancel.Click -= cancel_Click;
        }

        /// <summary>
        /// Override of Adroid OnTouchEvent.
        /// Closes the keyboard if a user touches outside of the input control.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(nameText.WindowToken, 0);
            return base.OnTouchEvent(e);
        }

        /// <summary>
        /// Handles clicks on the addTask button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTask_Click(object sender, EventArgs e)
        {
            ReportManager.Instance.AddReport(nameText.Text);
            base.OnBackPressed();
        }

        /// <summary>
        /// Handles clicks on the cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }
    }
}
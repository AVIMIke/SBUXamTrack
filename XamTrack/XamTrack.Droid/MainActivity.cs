using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace XamTrack.Droid
{
    /// <summary>
    /// This activity will show the user a list of all thier time reports as well as the report that is currently being tracked.
    /// </summary>
	[Activity (Label = "XamTrack.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        /// <summary>
        /// Override of Android Activity OnCreate
        /// </summary>
        /// <param name="bundle">The bundle that was passed when starting this activity.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);
		}
	}
}



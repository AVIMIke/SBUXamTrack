// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace XamTrack.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UILabel ActiveTaskCurrentTimeText { get; set; }

		[Outlet]
		UIKit.UILabel ActiveTaskText { get; set; }

		[Outlet]
		UIKit.UILabel ActiveTaskTotalTimeText { get; set; }

		[Outlet]
		UIKit.UIButton NewTaskButton { get; set; }

		[Outlet]
		UIKit.UITableView ReportList { get; set; }

		[Action ("CheeseClick:")]
		partial void CheeseClick (Foundation.NSObject sender);

		[Action ("CheeseEnter:")]
		partial void CheeseEnter (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ActiveTaskCurrentTimeText != null) {
				ActiveTaskCurrentTimeText.Dispose ();
				ActiveTaskCurrentTimeText = null;
			}

			if (ActiveTaskText != null) {
				ActiveTaskText.Dispose ();
				ActiveTaskText = null;
			}

			if (ActiveTaskTotalTimeText != null) {
				ActiveTaskTotalTimeText.Dispose ();
				ActiveTaskTotalTimeText = null;
			}

			if (NewTaskButton != null) {
				NewTaskButton.Dispose ();
				NewTaskButton = null;
			}

			if (ReportList != null) {
				ReportList.Dispose ();
				ReportList = null;
			}
		}
	}
}

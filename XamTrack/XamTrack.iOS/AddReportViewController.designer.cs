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
	[Register ("AddReportViewController")]
	partial class AddReportViewController
	{
		[Outlet]
		UIKit.UIButton CreateBtn { get; set; }

		[Outlet]
		UIKit.UITextField NameInput { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NameInput != null) {
				NameInput.Dispose ();
				NameInput = null;
			}

			if (CreateBtn != null) {
				CreateBtn.Dispose ();
				CreateBtn = null;
			}
		}
	}
}

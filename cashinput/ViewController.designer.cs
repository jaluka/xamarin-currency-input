// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace cashinput
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UILabel lbl_result { get; set; }

		[Outlet]
		cashinput.CurrencyTextField txf_bedrag1 { get; set; }

		[Outlet]
		cashinput.CurrencyTextField txf_bedrag2 { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txf_bedrag1 != null) {
				txf_bedrag1.Dispose ();
				txf_bedrag1 = null;
			}

			if (txf_bedrag2 != null) {
				txf_bedrag2.Dispose ();
				txf_bedrag2 = null;
			}

			if (lbl_result != null) {
				lbl_result.Dispose ();
				lbl_result = null;
			}
		}
	}
}

using System;
using MonoTouch.UIKit;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public class StatePickerSource : UIPickerViewModel
	{
		public StatePickerSource ()
		{


		}

		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return 1;
		}

		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}
	}
}


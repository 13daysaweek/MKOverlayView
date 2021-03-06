using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public class StatePickerViewModel : UIPickerViewModel
 	{
		private readonly IList<string> states;

		public event EventHandler<StatePickerChangedEventArgs> SelectedStateChanged;

		public StatePickerViewModel(IList<string> states)
		{
			this.states = states;
		}

		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return this.states.Count;
		}

		public override float GetRowHeight (UIPickerView picker, int component)
		{
			return 44f;
		}

		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			return this.states[row];
		}

		public override void Selected (UIPickerView picker, int row, int component)
		{
			if (this.SelectedStateChanged != null)
			{
				this.SelectedStateChanged(this, new StatePickerChangedEventArgs(this.states[row]));
			}
		}
	}

}


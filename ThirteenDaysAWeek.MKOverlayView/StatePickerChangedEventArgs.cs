using System;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace ThirteenDaysAWeek.MKOverlayView
{

	public class StatePickerChangedEventArgs : EventArgs
	{
		public StatePickerChangedEventArgs(string stateName)
		{
			this.SelectedState = stateName;
		}

		public string SelectedState {get; private set;}
	}
}

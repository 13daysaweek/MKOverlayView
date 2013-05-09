using System;
using MonoTouch.UIKit;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public static class DeviceExtensions
	{	
		public static float GetCurrentHeight(this UIApplication application)
		{
			float height=0f;
			UIInterfaceOrientation orient = application.StatusBarOrientation;

			if( (orient==UIInterfaceOrientation.PortraitUpsideDown)  
				 ||  (orient==UIInterfaceOrientation.Portrait))
			{
				height=UIScreen.MainScreen.Bounds.Height;
			}
			else
			{
				height=UIScreen.MainScreen.Bounds.Width;
			}

			return height;
		}
		
		public static float GetCurrentWidth(this UIApplication application)
		{
			float width=0f;
			UIInterfaceOrientation orient = application.StatusBarOrientation;

			if( (orient==UIInterfaceOrientation.PortraitUpsideDown)  
			   ||  (orient==UIInterfaceOrientation.Portrait))
			{
				width=UIScreen.MainScreen.Bounds.Width;
			}
			else
			{
				width=UIScreen.MainScreen.Bounds.Height;
			}

			return width;
		}
	}
}


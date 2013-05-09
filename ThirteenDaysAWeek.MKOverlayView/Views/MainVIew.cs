using System;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using System.Drawing;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public class MainVIew : UIView
	{
		public MainVIew (RectangleF frame) : base(frame)
		{
		}

		public override void LayoutSubviews ()
		{
			this.Toolbar = new UIToolbar(new RectangleF(0, 0, this.Frame.Width, this.Frame.Height));
			this.MapView = new MKMapView(new RectangleF(0, 44, this.Frame.Width, this.Frame.Height - 44));

			this.AddSubview (this.Toolbar);
			this.AddSubview(this.MapView);
		}

		public UIToolbar Toolbar {get; private set;}

		public MKMapView MapView {get; private set;}
	}
}


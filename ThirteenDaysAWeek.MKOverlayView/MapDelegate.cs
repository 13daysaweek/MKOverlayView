using System;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public class MapDelegate : MKMapViewDelegate
	{
		private float blue = .002f;

		public override MonoTouch.MapKit.MKOverlayView GetViewForOverlay (MKMapView mapView, NSObject overlay)
		{
			MKPolygon polygon = overlay as MKPolygon;
			MKPolygonView polygonView = new MKPolygonView(polygon);
			polygonView.FillColor = new UIColor(.2f, .9f, blue,1f);
			blue += .02f;
			return polygonView;
		}
	}
}


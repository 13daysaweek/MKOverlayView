using System;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public class MapDelegate : MKMapViewDelegate
	{
		public override MonoTouch.MapKit.MKOverlayView GetViewForOverlay (MKMapView mapView, NSObject overlay)
		{
			MKPolygon polygon = overlay as MKPolygon;
			MKPolygonView polygonView = new MKPolygonView(polygon);
			polygonView.FillColor = UIColor.Purple;

			return polygonView;
		}
	}
}


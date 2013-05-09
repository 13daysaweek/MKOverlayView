using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using ThirteenDaysAWeek.MKOverlayView.Models;
using System.Drawing;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public partial class ThirteenDaysAWeek_MKOverlayViewViewController : UIViewController
	{
		private IList<State> states;
		private MKMapView mapView;

		public ThirteenDaysAWeek_MKOverlayViewViewController () : base ("ThirteenDaysAWeek_MKOverlayViewViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.states = this.GetStates();
			this.SetupView();
			this.AddOverlays();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		private void SetupView()
		{
			this.mapView = new MKMapView(new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height - 44));
			this.View.AddSubview (mapView);
		}

		private void AddOverlays()
		{
			foreach (State state in this.states)
			{
				CLLocationCoordinate2D[] boundaryCoordinates = state.Boundary.Select(coord => new CLLocationCoordinate2D(coord.Latitude, coord.Longitude)).ToArray();
				MKPolygon statePolygon = MKPolygon.FromCoordinates(boundaryCoordinates);
				statePolygon.Title = state.Name;
				this.mapView.AddOverlay(statePolygon);
			}
		}

		private IList<State> GetStates()
		{
			string filePath = NSBundle.MainBundle.PathForResource("Content/states", "xml");
			XDocument document = XDocument.Load(filePath);
			
			var states = (from n in document.Descendants("state")
			              select new State
			              {
				Name = n.Attribute("name").Value,
				Boundary = (from s in n.Descendants("point")
				               select new Coordinates
				               {
					Latitude = double.Parse(s.Attribute("lat").Value),
					Longitude = double.Parse(s.Attribute("lng").Value)
				}).ToList()
			}).ToList();
			
			return states;
		}
	}
}


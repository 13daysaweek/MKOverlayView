using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ThirteenDaysAWeek.MKOverlayView.Models;

namespace ThirteenDaysAWeek.MKOverlayView
{
	public partial class ThirteenDaysAWeek_MKOverlayViewViewController : UIViewController
	{
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
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
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


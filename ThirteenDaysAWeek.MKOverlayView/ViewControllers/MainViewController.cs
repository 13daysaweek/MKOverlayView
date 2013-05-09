using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using ThirteenDaysAWeek.MKOverlayView.Models;
using ThirteenDaysAWeek.MKOverlayView.Views;

namespace ThirteenDaysAWeek.MKOverlayView.ViewControllers
{
	public partial class MainViewController : UIViewController
	{
		private IList<State> states;
		private MKPolygon currentStateOverlay;
		private MainView mainView;

		public override void LoadView ()
		{
			this.mainView = new MainView(new RectangleF(0, 
			                                       		0, 
			                                       		UIApplication.SharedApplication.GetCurrentWidth(), 
			                                       		UIApplication.SharedApplication.GetCurrentHeight()));
			this.View = mainView;
		}
	
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.states = this.GetStates();
			this.SetupPickerView();
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait | UIInterfaceOrientationMask.PortraitUpsideDown;
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			return UIInterfaceOrientation.Portrait;
		}

		private void SetupPickerView()
		{
			IList<string> stateList = this.states.Select(s => s.Name).ToList();
			StatePickerViewModel pickerViewModel = new StatePickerViewModel(stateList);
			pickerViewModel.SelectedStateChanged += (sender, e) => {
				this.OnStateSelected(e.SelectedState);
			};

			this.mainView.StatesPickerView.Model = pickerViewModel;
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

		private void OnStateSelected(string stateName)
		{
			if (this.currentStateOverlay != null)
			{
				this.mainView.MapView.RemoveOverlay(this.currentStateOverlay);
			}

			State selectedState = this.states.First(state => state.Name == stateName);		

			this.mainView.MoveMapIntoViewAndPickerOutOfView();

			CLLocationCoordinate2D[] stateBoundary = selectedState.Boundary.Select(coord => new CLLocationCoordinate2D(coord.Latitude, coord.Longitude)).ToArray();
			this.currentStateOverlay = MKPolygon.FromCoordinates(stateBoundary);
			this.mainView.MapView.AddOverlay (this.currentStateOverlay);
		}
	}
}


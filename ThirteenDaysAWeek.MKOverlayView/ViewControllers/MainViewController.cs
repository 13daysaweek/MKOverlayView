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
			// Initialize the view and store it in an instance field so we don't have to cast to MainView
			// later when we want to access specific fields/methods exposed by MainView
			this.mainView = new MainView(new RectangleF(0f, 
			                                       		0f, 
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

		/// <summary>
		/// Sets the Model property on the StatesPickerView exposed by the view
		/// </summary>
		private void SetupPickerView()
		{
			// Get a list of state names from our collection of states and coordinates
			IList<string> stateList = this.states.Select(s => s.Name).ToList();
			StatePickerViewModel pickerViewModel = new StatePickerViewModel(stateList);
			pickerViewModel.SelectedStateChanged += (sender, e) => {
				this.OnStateSelected(e.SelectedState);
			};

			this.mainView.StatesPickerView.Model = pickerViewModel;
		}

		/// <summary>
		/// Extracts state names and boundary coordinates from the bundled states.xml file
		/// </summary>
		/// <returns>A collection of State objects, containing state names and boundary coordinates</returns>
		private IList<State> GetStates()
		{
			string filePath = NSBundle.MainBundle.PathForResource("Content/states", "xml");
			XDocument document = XDocument.Load(filePath);
			
			IList<State> stateList = (from n in document.Descendants("state")
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
			
			return stateList;
		}

		private void OnStateSelected(string stateName)
		{
			// If there's already a state overlay on the map we need to remove it
			if (this.currentStateOverlay != null)
			{
				this.mainView.MapView.RemoveOverlay(this.currentStateOverlay);
			}

			State selectedState = this.states.First(state => state.Name == stateName);		

			// Hide the picker and move the map back into position
			this.mainView.MoveMapIntoViewAndPickerOutOfView();

			// Get an array of CLLocationCoordinate2D from the coordintes collection on the selected State object,
			// create a polygon and add it to the map...
			CLLocationCoordinate2D[] stateBoundary = selectedState.Boundary.Select(coord => new CLLocationCoordinate2D(coord.Latitude, coord.Longitude)).ToArray();
			this.currentStateOverlay = MKPolygon.FromCoordinates(stateBoundary);
			this.currentStateOverlay.Title = selectedState.Name;
			this.mainView.MapView.AddOverlay (this.currentStateOverlay);
		}
	}
}


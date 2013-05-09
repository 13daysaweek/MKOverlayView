using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using ThirteenDaysAWeek.MKOverlayView.Models;
using ThirteenDaysAWeek.MKOverlayView.Views;
using System.Drawing;

namespace ThirteenDaysAWeek.MKOverlayView.ViewControllers
{
	[Register ("MainViewController")]
	public partial class MainViewController : UIViewController
	{
		private IList<State> states;
		private MKMapView mapView;
		private UIPickerView statePicker;
		private UIToolbar toolbar;
		private MKPolygon currentStateOverlay;
		private MainView mainView;

		public MainViewController ()
		{
		}

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
//			this.SetupView();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
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

		private void SetupView()
		{
			this.toolbar = new UIToolbar(new RectangleF(0,0, this.View.Frame.Width, 44));
			this.View.AddSubview(toolbar);

			UIBarButtonItem statesButton = new UIBarButtonItem("States", UIBarButtonItemStyle.Bordered, (s,e) => {
				this.mainView.MoveMapOutOfViewAndPickerIntoView();
			});

			this.toolbar.SetItems(new UIBarButtonItem[]{statesButton}, true);


			this.statePicker = new UIPickerView(new RectangleF(0,500,this.View.Frame.Width, 180));
			IList<string> stateList = this.states.Select (s => s.Name).ToList();
			StatePickerViewModel pickerViewModel = new StatePickerViewModel(stateList);
			pickerViewModel.SelectedStateChanged += (sender, e) => {
				this.OnStateSelected(e.SelectedState);
			};
			this.statePicker.Model = pickerViewModel;
			this.statePicker.ShowSelectionIndicator = true;
			this.View.AddSubview(this.statePicker);

			this.mapView = new MKMapView(new RectangleF(0, 44, this.View.Frame.Width, this.View.Frame.Height -44));
			this.mapView.Delegate = new MapDelegate();
			this.View.AddSubview (mapView);
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


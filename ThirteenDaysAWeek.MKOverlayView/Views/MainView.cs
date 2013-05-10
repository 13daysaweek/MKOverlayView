using System;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;

namespace ThirteenDaysAWeek.MKOverlayView.Views
{
	public class MainView : UIView
	{
		private const double ANIMATION_DURATION = .4;
		private bool statePickerIsVisible;

		public MainView (RectangleF frame) : base(frame)
		{
			this.SetupView();
		}

		/// <summary>
		/// Initialize all the things and add them to the view
		/// </summary>
		private void SetupView()
		{
			this.Toolbar = new UIToolbar(new RectangleF(0f, 0f, this.Frame.Width, 44f));
			this.MapView = new MKMapView(new RectangleF(0f, 44f, this.Frame.Width, this.Frame.Height - 44f));
			this.MapView.Delegate = new MapDelegate();

			this.StatesPickerView = new UIPickerView(new RectangleF(900f, 44f, this.Frame.Width, 180f));
			this.StatesPickerView.ShowSelectionIndicator = true;

			UIBarButtonItem statesButton = new UIBarButtonItem("States", UIBarButtonItemStyle.Bordered, (s,e) => {
				this.MoveMapOutOfViewAndPickerIntoView();
			});
			this.Toolbar.Items = new UIBarButtonItem[]{statesButton};

			this.AddSubview (this.Toolbar);
			this.AddSubview(this.MapView);
			this.AddSubview(this.StatesPickerView);

			this.SetupGestureRecognizer();
		}

		public UIToolbar Toolbar {get; private set;}

		public MKMapView MapView {get; private set;}

		public UIPickerView StatesPickerView {get; private set;}

		public void MoveMapIntoViewAndPickerOutOfView()
		{
			UIView.BeginAnimations("overlay");
			UIView.SetAnimationDuration(ANIMATION_DURATION);
			
			this.MapView.Frame = new RectangleF(0f, 44f, this.Frame.Width, this.Frame.Height - 44f);
			this.StatesPickerView.Frame = new RectangleF(900f, 44f, this.Frame.Width, 180f);
			
			UIView.CommitAnimations();
			this.statePickerIsVisible = false;
		}
		
		public void MoveMapOutOfViewAndPickerIntoView()
		{
			if (!this.statePickerIsVisible)
			{
				UIView.BeginAnimations("moveMap");
				UIView.SetAnimationDuration(ANIMATION_DURATION);
			
				this.StatesPickerView.Frame = new RectangleF(0f, 44f, this.Frame.Width, 180f);
				this.MapView.Frame = new RectangleF(0f, 244f, this.MapView.Frame.Width, this.MapView.Frame.Height);
			
				UIView.CommitAnimations();
				this.statePickerIsVisible = true;
			}
		}

		private void SetupGestureRecognizer()
		{
			// We want to respond to a tap gesture on our polygon.  MKPolygonView supports gesture recognizers
			// however it appears to ignore them.  To get around that, we'll attach a gesture recognizer to the
			// MKMapView.  In the action for our gesture recognizer, we'll determine whether the tap was inside
			// of the polygon or not.  Note the looping through the Overlays collection on the MapView.  For this
			// app that isn't strictly necessary as we'll only ever have one overlay, however it's a good demonstration
			// of how you'd handle this with multiple overlays on your map
			this.MapView.AddGestureRecognizer(new UITapGestureRecognizer(r => {
				PointF pointInView = r.LocationInView(this.MapView);
				CLLocationCoordinate2D touchCoordinates = this.MapView.ConvertPoint(pointInView, this.MapView);
				MKMapPoint mapPoint = MKMapPoint.FromCoordinate(touchCoordinates);

				foreach (NSObject overlay in this.MapView.Overlays)
				{
					if (overlay is MKPolygon)
					{
						MKPolygon polygon = (MKPolygon)overlay;
						MonoTouch.MapKit.MKOverlayView view = this.MapView.ViewForOverlay(polygon);

						if (view is MKPolygonView)
						{
							MKPolygonView polygonView = (MKPolygonView)view;
							PointF polygonViewPoint = polygonView.PointForMapPoint(mapPoint);
							bool isInView = polygonView.Path.ContainsPoint(polygonViewPoint, false);

							if (isInView)
							{
								string message = string.Concat("You tapped ", polygon.Title);
								UIAlertView alertView = new UIAlertView("Tap recognized!", message, null, "OK");

								alertView.Show ();
							}
						}
					}
				}
			}));
		}
	}
}


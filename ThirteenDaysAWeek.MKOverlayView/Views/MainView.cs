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
		public MainView (RectangleF frame) : base(frame)
		{
			this.SetupView();
		}

		private void SetupView()
		{
			this.Toolbar = new UIToolbar(new RectangleF(0, 0, this.Frame.Width, this.Frame.Height));
			this.MapView = new MKMapView(new RectangleF(0, 44, this.Frame.Width, this.Frame.Height - 44));
			this.MapView.Delegate = new MapDelegate();

			this.StatesPickerView = new UIPickerView(new RectangleF(900,44,this.Frame.Width, 180));
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
			UIView.SetAnimationDuration(.3);
			
			this.MapView.Frame = new RectangleF(0, 44, this.Frame.Width, this.Frame.Height - 44);
			this.StatesPickerView.Frame = new RectangleF(900, 44, this.Frame.Width, 180);
			
			UIView.CommitAnimations();
		}
		
		public void MoveMapOutOfViewAndPickerIntoView()
		{
			UIView.BeginAnimations("moveMap");
			UIView.SetAnimationDuration(.3);
			
			this.StatesPickerView.Frame = new RectangleF(0, 44, this.Frame.Width, 180);
			this.MapView.Frame = new RectangleF(0, 244, this.MapView.Frame.Width, this.MapView.Frame.Height);
			
			UIView.CommitAnimations();
		}

		private void SetupGestureRecognizer()
		{
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


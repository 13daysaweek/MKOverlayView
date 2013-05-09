using System;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using System.Drawing;

namespace ThirteenDaysAWeek.MKOverlayView.Views
{
	public class MainView : UIView
	{
		public MainView (RectangleF frame) : base(frame)
		{
			this.SetupView();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			this.Toolbar.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
			this.MapView.Frame = new RectangleF(0, 44, this.Frame.Width, this.Frame.Height - 44);
			this.StatesPickerView.Frame = new RectangleF(0, 500, this.Frame.Width, 180);
		}

		private void SetupView()
		{
			this.Toolbar = new UIToolbar(new RectangleF(0, 0, this.Frame.Width, this.Frame.Height));
			this.MapView = new MKMapView(new RectangleF(0, 44, this.Frame.Width, this.Frame.Height - 44));
			this.MapView.Delegate = new MapDelegate();

			UIBarButtonItem statesButton = new UIBarButtonItem("States", UIBarButtonItemStyle.Bordered, (s,e) => {
				this.MoveMapOutOfViewAndPickerIntoView();
			});
			this.Toolbar.SetItems(new UIBarButtonItem[]{statesButton}, true);

			this.StatesPickerView = new UIPickerView(new RectangleF(0,500,this.Frame.Width, 180));
			this.StatesPickerView.ShowSelectionIndicator = true;

			this.AddSubview (this.Toolbar);
			this.AddSubview(this.MapView);
			this.AddSubview(this.StatesPickerView);
		}

		public UIToolbar Toolbar {get; private set;}

		public MKMapView MapView {get; private set;}

		public UIPickerView StatesPickerView {get; private set;}

		public void MoveMapIntoViewAndPickerOutOfView()
		{
			UIView.BeginAnimations("overlay");
			UIView.SetAnimationDuration(.3);
			
			this.MapView.Frame = new RectangleF(0, 44, this.Frame.Width, this.Frame.Height - 44);
			this.StatesPickerView.Frame = new RectangleF(0, 500, this.Frame.Width, 180);
			
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
	}
}


using System.Collections.Generic;

namespace ThirteenDaysAWeek.MKOverlayView.Models
{
	public class State
	{
		public State()
		{
			this.Boundary = new List<Coordinates>();
		}

		public string Name {get; set;}

		public IList<Coordinates> Boundary {get; set;}
	}
}


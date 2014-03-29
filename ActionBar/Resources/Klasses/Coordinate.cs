using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Coordinate
	{
		public float latitude {get; set;}
		public float longitude {get; set;}
		public Coordinate()
		{
		}
		public Coordinate(float latitude, float longitude)
		{
			this.latitude = latitude;
			this.longitude = longitude;
		}
	}
}
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Coordinate
	{
		public double latitude {get; set;}
		public double longitude {get; set;}
		public Coordinate()
		{
		}
		public Coordinate(double latitude, double longitude)
		{
			this.latitude = latitude;
			this.longitude = longitude;
		}
		public double distanceBetween(Coordinate pointB) //Haversine implementation
		{
			double R = 6371; //Radius of the earth in km
			double deltaLatitude = toRad(pointB.latitude) - toRad(this.latitude);
			double deltaLongitude = toRad(pointB.longitude) - toRad(this.longitude);


			double a = Math.Sin(deltaLatitude/2) * Math.Sin(deltaLatitude/2) + Math.Sin(deltaLongitude/2) * Math.Sin(deltaLongitude/2) * Math.Cos(toRad(this.latitude)) * Math.Cos(toRad(pointB.latitude));
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));

			return R * c * 1000;
		}
		private static double toRad(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
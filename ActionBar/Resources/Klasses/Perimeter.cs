using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Perimeter
	{
		Coordinate epicenter;
		double shortRange;
		double longRange;
		public Perimeter(Coordinate epicenter)
		{
			this.epicenter = epicenter;
			this.shortRange = 0.0;
			this.longRange = 0.0;
		}
		public Perimeter(Coordinate epicenter, double shortRange, double longRange)
		{
			this.epicenter = epicenter;
			this.shortRange = shortRange;
			this.longRange = longRange;
		}
		public int getZoneRating(Coordinate coordinate) // 0 = home, 1 = public, 2 = forbidden, 4 = error
		{
			double distanceBetween = this.epicenter.distanceBetween (coordinate);
			if (distanceBetween > 0 && distanceBetween <= shortRange)
			{
				return 0;
			} 
			else if (distanceBetween > shortRange && distanceBetween >= longRange)
			{
				return 1;
			} 
			else if (distanceBetween > longRange)
			{
				return 2;
			} 
			else
			{
				return 3;
			}

		}
	}
}
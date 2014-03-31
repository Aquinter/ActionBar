using System;
using System.Drawing;
using System.IO;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Perimeter
	{
		Coordinate epicenter;
		double shortRange;
		double longRange;
        string name;

		public Perimeter(Coordinate epicenter)
		{
			this.epicenter = epicenter;
			this.shortRange = 0.0;
			this.longRange = 0.0;
		}
		public Perimeter(string name, Coordinate epicenter, double shortRange, double longRange)
		{
            this.name = name;
			this.epicenter = epicenter;
			this.shortRange = shortRange;
			this.longRange = longRange;
		}
		public int getZoneRating(Coordinate coordinate) // 0 = home, 1 = public, 2 = forbidden, 4 = error
		{
			double dist = Math.Abs(this.epicenter.distanceBetween (coordinate));

			if (dist >= 0.0 && dist <= this.shortRange)
			{
				return 0;
			} 
			else if (dist > this.shortRange && dist <= this.longRange)
			{
				return 1;
			} 
			else if (dist > this.longRange)
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
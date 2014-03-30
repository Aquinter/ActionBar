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
<<<<<<< HEAD
			double distanceBetween = this.epicenter.distanceBetween (coordinate);
   if (distanceBetween >= 0 && distanceBetween <= shortRange)
=======
			double dist = Math.Abs(this.epicenter.distanceBetween (coordinate));

			if (dist >= 0.0 && dist <= this.shortRange)
>>>>>>> a9b99306a79d20c7b5c1389ed345645a6f0f4eb9
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
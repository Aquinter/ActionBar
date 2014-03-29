using System;
using System.Collections.Generic;
namespace IngHackaton
{
	public class TrustedLocations
	{
		List<Perimeter> trustedLocations;

		public TrustedLocations()
		{
				trustedLocations = new List<Perimeter>();
		}

		public int isTrusted(Coordinate coordinateToCheck)
		{
			int current = 5;

			foreach (Perimeter perimeter in trustedLocations)
			{
				int number = perimeter.getZoneRating(coordinateToCheck);
				if (number < current)
				{
					current = number;
				}
			}
			
				return current;
		}
		public void addPerimeter(Perimeter newPerimeter)
		{
			trustedLocations.Add(newPerimeter);
		}
		public void removePerimeter(Perimeter perimeterToRemove)
		{
			trustedLocations.RemoveAt(trustedLocations.IndexOf(perimeterToRemove));
		}
	}
}


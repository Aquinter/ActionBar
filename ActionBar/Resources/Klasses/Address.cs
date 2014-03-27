using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Address
	{
		private string street {get; set;}
		private int number {get; set;}
		private int bus {get; set;}
		private int zipCode {get; set;}
		private string city {get; set;}
		private string country {get; set;}
		private double latitude {get; set;}		//By convention: "+" for N, "-" for S
		private double longitude {get; set;}		//By convention: "+" for E, "-" for W
		private bool coordinatesSet {get; set;}

		public Address(string street, int postalNumber, int bus, int cityCode, string city, string country, int latitude, int longitude)
		{
            this.street = street;
            this.number = postalNumber;
            this.bus = bus;
            this.zipCode = cityCode;
            this.city = city;
            this.country = country;
			this.latitude = latitude;
            this.longitude = longitude;
            this.coordinatesSet = true;
		}
		public Address(string street, int postalNumber, int bus, int cityCode, string city, string country)
		{
			//Address(street, postalNumber, bus, cityCode, city, country, 0, 0);
            this.street = street;
            this.number = postalNumber;
            this.bus = bus;
            this.zipCode = cityCode;
            this.city = city;
            this.country = country;
            this.latitude = 0;
            this.longitude = 0;
            this.coordinatesSet = false;
		}
		public Address(string street, int postalNumber, int cityCode, string city, string country)
		{
			//Adress(street, postalNumber, 0, cityCode, city, country, 0, 0);
            this.street = street;
            this.number = postalNumber;
            this.bus = 0;
            this.zipCode = cityCode;
            this.city = city;
            this.country = country;
            this.latitude = 0;
            this.longitude = 0;
            this.coordinatesSet = false;
		}
		public void setCoordinates(double latitude, double longitude)
		{
            this.latitude = latitude;
            this.longitude = longitude;
            this.coordinatesSet = true;
		}
		public string toString()
		{
			return bus > 0 ? street + " " + number + " bus " + bus + "\n" + zipCode + " " + city + "\n" + country : street + " " + number + "\n" + zipCode + " " + city + "\n" + country;
		}
		public double distanceBetween(Address pointB) //Haversine implementation
		{
			if(this.coordinatesSet && pointB.coordinatesSet)
			{
				double R = 6371; //Radius of the earth in km
				double deltaLatitude = pointB.latitude - this.latitude;
				double deltaLongitude = pointB.longitude - this.longitude;


				double a = Math.Sin(deltaLatitude/2) * Math.Sin(deltaLatitude/2) + Math.Sin(deltaLongitude/2) * Math.Sin(deltaLongitude/2) * Math.Cos(toRad(this.latitude)) * Math.Cos(toRad(pointB.latitude));
				double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));

				return R * c;
			}
			return -1;
		}
		public int intDistanceBetween(Address pointB)
		{
			return (int)this.distanceBetween(pointB);
		}
		private static double toRad(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
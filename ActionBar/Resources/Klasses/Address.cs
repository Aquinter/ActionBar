using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Address
	{
		private string Street {get; set;}
		private int PostalNumber {get; set;}
		private int Bus {get; set;}
		private int CityCode {get; set;}
		private string City {get; set;}
		private string Country {get; set;}
		private double Latitude {get; set;}		//By convention: "+" for N, "-" for S
		private double Longitude {get; set;}		//By convention: "+" for E, "-" for W
		private bool CoordinatesSet {get; set;}

		public Address(string street, int postalNumber, int bus, int cityCode, string city, string country, int latitude, int longitude)
		{
			Street = street;
			PostalNumber = postalNumber;
			Bus = bus;
			CityCode = cityCode;
			City = city;
			Country = country;
			Latitude = latitude;
			Longitude = longitude;
			CoordinatesSet = true;
		}
		public Address(string street, int postalNumber, int bus, int cityCode, string city, string country)
		{
			//Address(street, postalNumber, bus, cityCode, city, country, 0, 0);
			Street = street;
			PostalNumber = postalNumber;
			Bus = bus;
			CityCode = cityCode;
			City = city;
			Country = country;
			Latitude = 0;
			Longitude = 0;
			CoordinatesSet = false;
		}
		public Address(string street, int postalNumber, int cityCode, string city, string country)
		{
			//Adress(street, postalNumber, 0, cityCode, city, country, 0, 0);
			Street = street;
			PostalNumber = postalNumber;
			Bus = 0;
			CityCode = cityCode;
			City = city;
			Country = country;
			Latitude = 0;
			Longitude = 0;
			CoordinatesSet = false;
		}
		public void setCoordinates(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
			CoordinatesSet = true;
		}
		public string toString()
		{
			return Bus > 0 ? Street + " " + PostalNumber + " bus " + Bus + "\n" + CityCode + " " + City + "\n" + Country : Street + " " + PostalNumber + "\n" + CityCode + " " + City + "\n" + Country;
		}
		public double distanceBetween(Adress pointB) //Haversine implementation
		{
			if(this.CoordinatesSet && pointB.CoordinatesSet)
			{
				double R = 6371; //Radius of the earth in km
				double deltaLatitude = pointB.Latitude - this.Latitude;
				double deltaLongitude = pointB.Longitude - this.Longitude;


				double a = Math.Sin(deltaLatitude/2) * Math.Sin(deltaLatitude/2) + Math.Sin(deltaLongitude/2) * Math.Sin(deltaLongitude/2) * Math.Cos(toRad(this.Latitude)) * Math.Cos(toRad(pointB.Latitude));
				double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));

				return R * c;
			}
			return -1;
		}
		public int intDistanceBetween(Adress pointB)
		{
			return (int)this.distanceBetween(pointB);
		}
		private static double toRad(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class DeviceId
	{
		string deviceId;
		public DeviceId()
		{
			NSUuid identifier = UIDevice.CurrentDevice.IdentifierForVendor;
			deviceId = identifier.ToString();
		}

		public string getDeviceId()
		{
			return deviceId;
		}	
	}
}
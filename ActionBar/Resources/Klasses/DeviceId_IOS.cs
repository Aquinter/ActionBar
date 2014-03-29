using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
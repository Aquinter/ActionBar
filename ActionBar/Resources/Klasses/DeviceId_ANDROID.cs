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
			var telephonyManager = (TelephonyManager) GetSystemService(TelephonyService);
			var id = telephonyManager.DeviceId;
			deviceId = id.ToString();
		}
		public string getDeviceId()
		{
			return deviceId;
		}	
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class DeviceId()
	{
		string deviceId;
		public DeviceId()
		{
			#if(IOS)
				NSUuid identifier = UIDevice.CurrentDevice.IdentifierForVendor;
				deviceId = identifier.ToString();
				//Console.WriteLine(identifier.ToString()); // For debug
			#endif

			#if(ANDROID)
				//Implementation for getting the IMEI (or other fixed code)

				//Nick: kan je dit eens testen aub?
				var telephonyManager = (TelephonyManager) GetSystemService(TelephonyService);
				var id = telephonyManager.DeviceId;
				deviceId = id.ToString();
			#endif
		}	
	}
}
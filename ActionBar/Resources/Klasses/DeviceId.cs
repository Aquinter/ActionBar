<<<<<<< HEAD
#if (ANDROID)
using Android.Content;
using Android.Telephony;
#endif

=======
//Obsolete

/*
>>>>>>> eeb94f95f5447108116f1ac2b6f16c1a1f5d061a
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class DeviceId
	{
		string deviceId = "ID here";
#if (ANDROID)
        Context c;
#endif

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

		public string getDeviceId()
		{
			return deviceId;
		}	
	}
}
*/

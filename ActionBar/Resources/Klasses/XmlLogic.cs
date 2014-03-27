using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class XmlLogic
	{
		string xmlString;
		Dictionary<string, string> xmlDataList;
		string md5hash;

		public XmlLogic()
		{		
		}
		public XmlLogic(string xmlString)
		{
			this.xmlString = xmlString;
		}
		public XmlLogic(Dictionary<string, string> xmlDataList)
		{
			this.xmlDataList = xmlDataList;
		}

		public void readXML() //Danku Nick
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                /*
                TextView[] txt = new TextView[8];
                String[] txtStr = new String[12];
                txt[0] = FindViewById<TextView>(Resource.Id.duedate);
                txt[1] = FindViewById<TextView>(Resource.Id.amount);
                txt[2] = FindViewById<TextView>(Resource.Id.firmname);
                txt[3] = FindViewById<TextView>(Resource.Id.firmaddress1);
                txt[4] = FindViewById<TextView>(Resource.Id.firmaddress2);
                txt[5] = FindViewById<TextView>(Resource.Id.iban);
                txt[6] = FindViewById<TextView>(Resource.Id.bic);
                txt[7] = FindViewById<TextView>(Resource.Id.reference);
                */
				xmlDataList = new Dictionary<String, String>(20);

                reader.ReadToFollowing("payment");

                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToNextAttribute();
					xmlDataList.Add(reader.Name, reader.Value);
                }
            }
        }

		public Payment parseXmlToPayment() // Danku Nick
        {
            string duedate = "";
            string amount = "";
            string currency = "";
            string firmname = "";
            string firmaddress = "";
            string iban = "";
            string bic = "";
            string reference = "";
            string referencetype = "";
            md5hash = "";

			xmlDataList.TryGetValue("currency", out currency);
			xmlDataList.TryGetValue("firmname", out firmname);
			xmlDataList.TryGetValue("iban", out iban);
			xmlDataList.TryGetValue("bic", out bic);
			xmlDataList.TryGetValue("reference", out reference);
			xmlDataList.TryGetValue("referencetype", out referencetype);
			xmlDataList.TryGetValue("duedate", out duedate);
			xmlDataList.TryGetValue("amount", out amount);
			xmlDataList.TryGetValue("address", out firmaddress);
			xmlDataList.TryGetValue("md5hash", out md5hash);

            string stringToHash = duedate + amount + currency + firmname + firmaddress + iban + bic + reference + referencetype;
			Console.WriteLine (stringToHash);
			if (validateXml(stringToHash))
            {
            	Payment newPayment = new Payment(Convert.ToDateTime(duedate, new CultureInfo("ru-RU")), currency, Convert.ToDecimal(amount), iban, bic, reference, referencetype);
            	return newPayment;
            }
            return null;
        }

		private bool validateXml(string source)
        {
			//md5hash.Equals(CalculateMD5Hash(stringToHash));
			using (MD5 md5Hash = MD5.Create())
			{
				//string hash = GetMd5Hash(md5Hash, source);
				return VerifyMd5Hash (md5Hash, source, md5hash);
			}
        }

		//From MSDN
		static string GetMd5Hash(MD5 md5Hash, string input)
		{

			// Convert the input string to a byte array and compute the hash. 
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes 
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data  
			// and format each one as a hexadecimal string. 
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string. 
			return sBuilder.ToString();
		}

		// Verify a hash against a string. 

		//From MSDN
		static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
		{
			// Hash the input. 
			string hashOfInput = GetMd5Hash(md5Hash, input);

			// Create a StringComparer an compare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if (0 == comparer.Compare(hashOfInput, hash))
			{
				Console.WriteLine ("MD5 is correct");
				return true;
			}
			else
			{
				Console.WriteLine ("MD5 is unvalid");
				return false;
			}
		}
	}
}
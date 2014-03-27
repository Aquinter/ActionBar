using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Iban
	{
		string iban;
		public Iban(string iban)
		{
			this.iban = iban;
		}
		public bool validateIban() //returns false if iban is unvalid
		{
                  //			
			/*Validating the IBAN[edit]
			An IBAN is validated by converting it into an integer and performing a basic mod-97 operation (as described in ISO 7064) on it. If the IBAN is valid, the remainder equals 1.[Note 1] The algorithm of IBAN validation is as follows:[8]

			Check that the total IBAN length is correct as per the country. If not, the IBAN is invalid
			Move the four initial characters to the end of the string
			Replace each letter in the string with two digits, thereby expanding the string, where A = 10, B = 11, ..., Z = 35
			Interpret the string as a decimal integer and compute the remainder of that number on division by 97
			If the remainder is 1, the check digit test is passed and the IBAN might be valid.

			Example (fictitious United Kingdom bank, sort code 12-34-56, account number 98765432):

			• IBAN:		GB82 WEST 1234 5698 7654 32	
			• Rearrange:		W E S T12345698765432 G B82	
			• Convert to integer:		3214282912345698765432161182	
			• Compute remainder:		3214282912345698765432161182	mod 97 = 1
			
                  Code taken from rosettacode.org
                  */

      		if (string.IsNullOrEmpty(iban))
      	      {    
      	            return false;
      		}
      		if (iban.Length < 2)
      		{
                      return false;
                  }
                  var countryCode = iban.Substring(0, 2).ToUpper();
                  var lengthForCountryCode = _lengths[countryCode];
                  // Check length.
                  if (iban.Length < lengthForCountryCode)
                  {
                  	return false;
                  }
                  if (iban.Length > lengthForCountryCode)
                  {
      				return false;
                  }
                  iban = iban.ToUpper();
                  var newIban = iban.Substring(4) + iban.Substring(0, 4);
       
      			newIban = Regex.Replace(newIban, @"\D", match => ((int) match.Value[0] - 55).ToString());
       
      			var remainder = long.Parse(newIban) % 97;
       
                  if (remainder != 1)
                  {
                  	return false;
                  }
                  else
                  {
                  	return true;
                  }    
		}
		public string toString()
		{
			return "";
		}
		private static Dictionary<string, int> _lengths = new Dictionary<string, int>
            {
            {"AL", 28}, {"AD", 24}, {"AT", 20}, {"AZ", 28}, {"BE", 16},
            {"BH", 22}, {"BA", 20}, {"BR", 29}, {"BG", 22}, {"CR", 21},
            {"HR", 21}, {"CY", 28}, {"CZ", 24}, {"DK", 18}, {"DO", 28},
            {"EE", 20}, {"FO", 18}, {"FI", 18}, {"FR", 27}, {"GE", 22},
            {"DE", 22}, {"GI", 23}, {"GR", 27}, {"GL", 18}, {"GT", 28},
            {"HU", 28}, {"IS", 26}, {"IE", 22}, {"IL", 23}, {"IT", 27},
            {"KZ", 20}, {"KW", 30}, {"LV", 21}, {"LB", 28}, {"LI", 21},
            {"LT", 20}, {"LU", 20}, {"MK", 19}, {"MT", 31}, {"MR", 27},
            {"MU", 30}, {"MC", 27}, {"MD", 24}, {"ME", 22}, {"NL", 18},
            {"NO", 15}, {"PK", 24}, {"PS", 29}, {"PL", 28}, {"PT", 25},
            {"RO", 24}, {"SM", 27}, {"SA", 24}, {"RS", 22}, {"SK", 24},
            {"SI", 19}, {"ES", 24}, {"SE", 24}, {"CH", 21}, {"TN", 24},
            {"TR", 26}, {"AE", 23}, {"GB", 22}, {"VG", 24}
            };
	}
}
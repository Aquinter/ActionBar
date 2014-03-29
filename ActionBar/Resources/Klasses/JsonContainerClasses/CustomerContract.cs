using System.Collections.Generic;

namespace Aping
{
    public class CustomerContract
    {
        public List<Phone> phones { get; set; }
        public List<Address> addresses { get; set; }
        public List<EmailAddress> emailAddresses { get; set; }
    }

    public class Phone
    {
        public string phoneNumber { get; set; }
        public string lastUpdateDate { get; set; }
        public string phoneType { get; set; }
    }

    public class Address
    {
        public string address { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string streetType { get; set; }
        public string province { get; set; }
        public string provinceCode { get; set; }
        public string zipCode { get; set; }
        public string countryCode { get; set; }
        public string addressType { get; set; }
        public bool mailingAddress { get; set; }
        public string streetTypeCode { get; set; }
        public string addressTypeCode { get; set; }
    }

    public class EmailAddress
    {
        public string email { get; set; }
    }
}

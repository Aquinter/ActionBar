using System.Collections.Generic;

namespace Aping
{
    public class ProductsList
    {
        public string uuid { get; set; }
        public List<Holder> holders { get; set; }
        public double assignedOverdraftLimit { get; set; }
        public string name { get; set; }
        public string productNumber { get; set; }
        public string bank { get; set; }
        public string iban { get; set; }
        public string bic { get; set; }
        public string openingDate { get; set; }
        public int type { get; set; }
        public string subtype { get; set; }
        public double availableBalance { get; set; }
        public double balance { get; set; }
    }

    public class InterventionDegree
    {
        public string description { get; set; }
        public string code { get; set; }
    }

    public class Holder
    {
        public string name { get; set; }
        public InterventionDegree interventionDegree { get; set; }
    }
}

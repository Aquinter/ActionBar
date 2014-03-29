using System.Collections.Generic;

namespace Aping
{
    public class UpdateTransfer
    {
        public TransferTransaction transferTransaction { get; set; }
        public AcceptanceMethod acceptanceMethod { get; set; }
    }
    public class From
    {
        public string uuid { get; set; }
        public string productNumber { get; set; }
    }

    public class To
    {
        public string productNumber { get; set; }
        public string titular { get; set; }
        public string bank { get; set; }
    }

    public class TransferTransaction
    {
        public double amount { get; set; }
        public int expensesAmount { get; set; }
        public string expensesType { get; set; }
        public int commission { get; set; }
        public string concept { get; set; }
        public string currency { get; set; }
        public string effectiveDate { get; set; }
        public string operationDate { get; set; }
        public string receptionDate { get; set; }
        public From from { get; set; }
        public To to { get; set; }
    }

    public class AcceptanceMethod
    {
        public string message { get; set; }
        public string code { get; set; }
        public bool nextAcceptanceMethod { get; set; }
        public List<int> pinPositions { get; set; }
        public string validationType { get; set; }
    }
}

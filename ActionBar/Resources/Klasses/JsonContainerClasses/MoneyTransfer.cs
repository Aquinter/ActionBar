namespace Aping
{
    public class MoneyTransfer
    {
        public From from { get; set; }
        public To to { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string operationDate { get; set; }
        public string concept { get; set; }
    }
}

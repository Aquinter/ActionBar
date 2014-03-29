namespace Aping
{
    public class CustomerFinancialInformation
    {
        public LaboralActivity laboralActivity { get; set; }
        public string companyName { get; set; }
    }

    public class LaboralActivity
    {
        public int laboralStatus { get; set; }
        public int professionType { get; set; }
        public int activitySector { get; set; }
        public string laboralStatusDesc { get; set; }
        public string professionTypeDesc { get; set; }
        public string activitySectorDesc { get; set; }
    }
}

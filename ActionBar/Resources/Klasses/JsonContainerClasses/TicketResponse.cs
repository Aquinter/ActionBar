namespace Aping
{
    public class TicketResponse
    {
        public string ticket { get; set; }
        public int timeoutInSeconds { get; set; }
        public string rememberMeToken { get; set; }
    }

    public class TicketBody
    {
        public string ticket { get; set; }
    }
}

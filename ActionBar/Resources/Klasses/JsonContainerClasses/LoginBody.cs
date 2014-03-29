namespace Aping
{
    public class LoginBody
    {
        public LoginDocument loginDocument { get; set; }
        public string birthday { get; set; }
    }

    public class LoginDocument
    {
        public int documentType { get; set; }
        public string document { get; set; }
    }
}

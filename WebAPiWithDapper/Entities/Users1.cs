namespace WebAPiWithDapper.Entities
{
    public class Users1
    {
        public string user1 { get; set; }
        public string password { get; set; }
        public string TokenId { get; set;}
    }

    public class Jwt
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}

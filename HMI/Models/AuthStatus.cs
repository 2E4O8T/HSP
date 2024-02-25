namespace HMI.Models
{
    public class AuthStatus
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}

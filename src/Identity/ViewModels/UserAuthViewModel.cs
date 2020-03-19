using System;

namespace Identity.ViewModels
{
    public class UserAuthViewModel
    {
        public string Token { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public string TokenType { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
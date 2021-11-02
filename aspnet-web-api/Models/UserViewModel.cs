namespace aspnet_web_api.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public UserViewModel(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public UserViewModel(string name, string email, string token)
        {
            Name = name;
            Email = email;
            Token = token;
        }

    }
}

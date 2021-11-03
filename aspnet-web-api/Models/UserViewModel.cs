namespace aspnet_web_api.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public UserViewModel(string name, string email, string role)
        {
            Name = name;
            Email = email;
            Role = role;
        }
        public UserViewModel(string name, string email, string role, string token)
        {
            Name = name;
            Email = email;
            Token = token;
            Role = role;
        }

    }
}

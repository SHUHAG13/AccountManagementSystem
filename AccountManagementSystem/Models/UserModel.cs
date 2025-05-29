namespace AccountManagementSystem.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime DateofJoining { get; set; }
    }
}

namespace ApiOrderFood.DTO
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}

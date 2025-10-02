namespace desawebback.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
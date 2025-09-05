namespace UsersInfoManagement_API.Dtos.UsersInfo
{
    public class UsersInfoReadDto
    {
        public int UsersInfoID { get; set; }
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int UserID { get; set; }
    }
}

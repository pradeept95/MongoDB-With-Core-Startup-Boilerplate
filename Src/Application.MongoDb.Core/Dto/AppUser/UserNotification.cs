namespace Application.Core.Dto.AppUser
{
    public class UserNotification
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Remarks { get; set; }
        public string Timestamp { get; set; }
        public string UserFullName { get; set; }
        public string UserId { get; set; } 
        public string DetailUrl { get; set; }
        public bool IsRead { get; set; }  
    }
}

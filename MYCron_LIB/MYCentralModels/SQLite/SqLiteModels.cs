using System.ComponentModel.DataAnnotations.Schema;

namespace MYCentralModels.SQLite
{
    [Table("UserSessions")]
    public class UserSession
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? EMail { get; set; }
        public string? Phone { get; set; }
        public DateTime LastSignedInOn { get; set; }
        public int UserType { get; set; }
        public DateTime LastAccessTime { get; set; }
    }
    [Table("OutOfSessionPages")]
    public class OutOfSessionPage
    {
        public int Id { get; set; }
        public string? PagePath { get; set; }
    }
    [Table("InternalUrls")]
    public class InternalUrl
    {
        public int Id { get; set; }
        public int ApiType { get; set; }
        public string? ApiUrl { get; set; }
    }

}

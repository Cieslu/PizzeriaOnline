#nullable disable

namespace MyWebApplication.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserData> UserDatas { get; set; }
    }
}

using MessageBoardClassLibrary.Models;

namespace AdminPortal.Models
{
    public class AdminViewModel : UserViewModel
    {
        public List<User>? Users { get; set; }
        public List<Area>? Areas { get; set; }
        public List<Field>? Fields { get; set; }
        public List<SchoolUser>? SchoolUsers { get; set; }
        public User? EditUser { get; set; }
        public School? EditSchool { get; set; }
        public string SchoolName { get; set; }
        public string AreaId { get; set; }


    }
}

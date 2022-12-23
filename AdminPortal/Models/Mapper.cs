using MessageBoardClassLibrary.Models;
using static AdminPortal.Models.PasswordHasher;

namespace AdminPortal.Models
{
    public static class Mapper
    {
        public static User MapToUser(this UserViewModel userViewModel) => new() { Email = userViewModel.Email, FirstName = userViewModel.FirstName, LastName = userViewModel.LastName, Id = Guid.NewGuid().ToString(), Role = userViewModel.Role, IsValidated = false };
        public static UserViewModel MapToUserViewModel(this User user) => user != null ? new() { Email = user.Email, Password = user.Password, FirstName = user.FirstName, LastName = user.LastName, ConfirmEmail = user.Email, ConfirmPassword = user.Password, Role = user.Role } : null;
        public static SchoolViewModel MapToSchoolViewModel(this School school) => school != null ? new() { Id = school.Id, Name = school.Name, AreaId = school.Id } : null;

    }
}

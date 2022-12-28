using MessageBoardClassLibrary.Models;
using static AdminPortal.Models.PasswordHasher;

namespace AdminPortal.Models
{
    public static class Mapper
    {
        public static User MapToUser(this UserViewModel userViewModel) => new() { Id = Guid.NewGuid().ToString(), Email = userViewModel.Email, FirstName = userViewModel.FirstName, LastName = userViewModel.LastName, Role = userViewModel.Role};
        public static UserViewModel MapToUserViewModel(this User user) => user != null ? new () { Email = user.Email, Password = user.Password, FirstName = user.FirstName, LastName = user.LastName, ConfirmEmail = user.Email, ConfirmPassword = user.Password, Role = user.Role } : null;
        public static SchoolViewModel MapToSchoolViewModel(this School school) => school != null ? new() { Id = school.Id, Name = school.Name, AreaId = school.Id } : null;
        public static Post MapToPost(this PostViewModel postViewModel) => postViewModel != null ? new () { Id = Guid.NewGuid().ToString(), DatePosted = postViewModel.DatePosted,Title = postViewModel.Title, Description = postViewModel.Description, Media = postViewModel.Media, User = postViewModel.User, IsValid = postViewModel.IsValid, IsVisible = postViewModel.IsVisible} : null;

    }
}

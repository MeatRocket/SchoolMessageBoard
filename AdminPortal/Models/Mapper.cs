using MessageBoardClassLibrary.Models;
using static AdminPortal.Models.PasswordHasher;

namespace AdminPortal.Models
{
    public static class Mapper
    {
        public static User MapToUser(this UserViewModel userViewModel) => new() { Id = Guid.NewGuid().ToString(), Email = userViewModel.Email, FirstName = userViewModel.FirstName, LastName = userViewModel.LastName, Role = userViewModel.Role};
        public static UserViewModel MapToUserViewModel(this User user) => user != null ? new () { Email = user.Email, Password = user.Password, FirstName = user.FirstName, LastName = user.LastName, ConfirmEmail = user.Email, ConfirmPassword = user.Password, Role = user.Role } : null;
        public static SchoolViewModel MapToSchoolViewModel(this School school) => school != null ? new() { Id = school.Id, Name = school.Name, AreaId = school.Id } : null;
        public static Post MapToPost(this PostViewModel postViewModel) => postViewModel != null ? new () { Id = Guid.NewGuid().ToString(), DatePosted = postViewModel.DatePosted,Title = postViewModel.Title, Description = postViewModel.Description, Media = postViewModel.Media, User = postViewModel.User, IsValid = postViewModel.IsValid, IsVisible = postViewModel.IsVisible, Template = postViewModel.Template} : null;
        public static PostViewModel MapToPostView(this Post post) => post != null ? new() { Id = post.Id, DatePosted = post.DatePosted, Title = post.Title, Description = post.Description, Media = post.Media, User = post.User, IsValid = post.IsValid, IsVisible = post.IsVisible, Template = post.Template } : null;
        public static Template TemplateClone(this Template template) => template != null ? new() { Id = template.Id, TemplateName = template.TemplateName, DynamicProperties = template.DynamicProperties } : null;
        //public static List<DynamicProperty> DynamicPropertiesClone(this List<DynamicProperty> dynamicProperties) => dynamicProperties.Select(x => new DynamicProperty(){ Id = Guid.NewGuid().ToString(), PropertyName = x.PropertyName, Sequence = x.Sequence, Type = x.Type });

    }
}
